// const baseAddress = window.location.hostname == "localhost" ? 
//             "http://localhost:7071" : 
//             "https://remindjames.azurewebsites.net";
const baseAddress = "https://remindjames.azurewebsites.net";

var app = new Vue({
    el: '#app',
    data: {
        reminders: [ ],
        error: undefined,
        now: moment()
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

            var next = _.find(ordered, function(r) { return r.hourInt > +curHour; });

            // restart loop if we couldn't find, else return empty if we're not yet mounted
            next = next ||  ordered[0] || {message: "", hourFormatted: ""}

            return next;
        }
    },
    methods: {
        updateReminder: function(reminder) {
            const body = JSON.stringify({ message: reminder.message });

            fetch(`${baseAddress}/api/reminder/${reminder.hour}`, 
                { method: "PUT", body: body })
                .then(response => this.getReminders())
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
        this.getReminders();
    },
});