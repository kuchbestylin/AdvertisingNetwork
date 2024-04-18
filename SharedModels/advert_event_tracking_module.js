
// Create a new script element
var script = document.createElement('script');

// Set the src attribute to the URL of the external JavaScript file
script.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.8/signalr.min.js';

// Append the script element to the document's <head> or <body>
document.head.appendChild(script);


// Define a JavaScript module for event tracking
const EventTracker = (function () {
    // Initialize SignalR connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/eventHub") // Adjust the URL as needed
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Start the SignalR connection
    connection.start().then(() => {
        console.log("Connection established");
    }).catch(err => {
        console.error(err.toString());
    });

    // Function to send event to the server
    function sendEvent(eventType, eventData) {
        connection.invoke("SendEvent", eventType, eventData).catch(err => {
            console.error(err.toString());
        });
    }

    // Function to attach event listeners to ad elements
    function attachEventListenersToAds() {
        const adElements = document.getElementsByClassName('ad'); // Adjust selector as needed
        for (let i = 0; i < adElements.length; i++) {
            adElements[i].addEventListener('click', function (event) {
                sendEvent('adClick', { adId: event.target.id }); // Send ad click event
            });

            // Add other event listeners as needed (e.g., impression, hover, etc.)
            // Example: adElements[i].addEventListener('impression', function(event) { ... });
        }
    }

    // Initialize the event tracker
    function init() {
        attachEventListenersToAds();
        // Add other initialization logic if needed
    }

    // Expose the init function publicly
    return {
        init: init
    };
})();

// Initialize the event tracker when the DOM is ready
document.addEventListener('DOMContentLoaded', function () {
    EventTracker.init();
});
