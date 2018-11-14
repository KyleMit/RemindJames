const baseAddress = 'https://remindjames.azurewebsites.net'; //'https://remindjames.azurewebsites.net/api/reminder'; //'http://localhost:7071';

var app = new Vue({
    el: '#app',
    data: {
        reminders: [
            // {hour: '8 am', message: 'Wake Up!'},
            // {hour: '9 am', message: 'Wake up Now!'}
        ],
        error: undefined
    },
    computed: {
        // https://stackoverflow.com/a/40512856/1366033
        orderedReminders: function () {
            return _.orderBy(this.reminders, 'hourInt');
        }
    },
    methods: {
        updateTodo: function(todo) {
            const body = JSON.stringify({ isCompleted: todo.isCompleted });
            fetch(`${baseAddress}/api/todo/${todo.id}`, 
                { method: "PUT", body: body,
                credentials: "same-origin"})
                .catch(reason => this.error = `Failed to update item: ${reason}`);
        },     
    },
    mounted: function () {
        fetch(`${baseAddress}/api/reminder`, {})
            .then(response => response.json())
            .then(json => this.reminders = json)
            .catch(reason => this.error = `Failed to fetch reminders: ${reason}`);
    },
});