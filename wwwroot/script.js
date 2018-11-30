const baseAddress = window.location.hostname == "localhost" ? 
            "http://localhost:7071" : 
            "https://remindjames.azurewebsites.net";
//const baseAddress = "https://remindjames.azurewebsites.net";

var app = new Vue({
    el: '#app',
    data: {
        reminders: [ ],
        error: undefined,
        now: moment(),
    },
    computed: {
        // https://stackoverflow.com/a/40512856/1366033
        orderedReminders: function () {
            return _.orderBy(this.reminders, 'hourSort');
        },
        nextReminder: function () {
            var nextHour = this.now.add(1, 'hours').format("HH") // inovke to set of computed, but use our wn
            var curHour = moment().format("HH")
            var ordered = _.orderBy(this.reminders, 'hourSort');

            var next = _.find(ordered, function(r) { return r.hourInt > +curHour & r.message != ""; });

            // restart loop if we couldn't find, else return empty if we're not yet mounted
            next = next ||  ordered[0];

            return next;
        }
    },
    watch: {
        reminders: {
            handler: function() {
            console.log('Reminders changed!');
            localStorage.setItem('reminders', JSON.stringify(this.reminders));
            },
            deep: true,
        },
    },
    methods: {
        updateReminder: function(reminder) {
            const body = JSON.stringify({ message: reminder.message });
            fetch(`${baseAddress}/api/reminder/${reminder.hour}`, 
                { method: "PUT", body: body })
                .catch(reason => this.error = `Failed to update item: ${reason}`);
        },     
        isNext(reminder) {
            return reminder.hour == this.nextReminder.hour ? "next" : "";
        },
        getReminders: function() {
            fetch(`${baseAddress}/api/reminder`, {})
            .then(response => response.json())
            .then(json => this.reminders = json)
            .catch(reason => this.error = `Failed to fetch reminders: ${reason}`);
        }
    },
    created: function(){
        setInterval(() => this.now = moment(), 1000 * 10)
    },
    mounted: function () {
        console.log('App mounted!');
        // fetch from local storage
        if (localStorage.getItem('reminders')) this.reminders = JSON.parse(localStorage.getItem('reminders'));

        // update from live stream
        this.getReminders();
    },

});


getConnectionInfo().then(info => {
    // make compatible with old and new SignalRConnectionInfo
    info.accessToken = info.accessToken || info.accessKey;
    info.url = info.url || info.endpoint;

    const options = {
      accessTokenFactory: () => info.accessToken
    };

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(info.url, options)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.on('updateReminder', updateReminder);

    connection.onclose(() => console.log('disconnected'));

    console.log('connecting...');

    connection.start()
      .then(() => console.log('connected!'))
      .catch(console.error);

  }).catch(console.error);

function getConnectionInfo() {
    return axios.post(`${baseAddress}/api/negotiate`, null, getAxiosConfig())
      .then(resp => resp.data);
}

function getAxiosConfig() {
    const config = {
        headers: {}
    };
    return config;
}

function updateReminder(updatedReminder) {
    var existingReminder = _.find(app.reminders, function(r) { return r.hourInt == updatedReminder.HourInt; });
    existingReminder.message = updatedReminder.Message;
    console.log("Socket responded with updated reminder", updatedReminder)
  }