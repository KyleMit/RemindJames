const baseAddress = window.location.hostname == "localhost" ? 
            "http://localhost:7071" : 
            "https://remindjames.azurewebsites.net";

var app = new Vue({
    el: '#app',
    data: {
        reminders: [ ],
        error: undefined
    },
    computed: {
        // https://stackoverflow.com/a/40512856/1366033
        orderedReminders: function () {
            return _.orderBy(this.reminders, 'hourSort');
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
        getReminders: function() {
            fetch(`${baseAddress}/api/reminder`, {})
            .then(response => response.json())
            .then(json => this.reminders = json)
            .catch(reason => this.error = `Failed to fetch reminders: ${reason}`);
        }
    },
    mounted: function () {
        this.getReminders();
    },
});