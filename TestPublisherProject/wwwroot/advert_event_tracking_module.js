
function initAdvertisingJs() {
    console.log("1");
    class AlmaicAdvert extends HTMLElement {
        constructor() {
            super();
        }
    }
    customElements.define("almaic-prologue", AlmaicAdvert);
    const adContainer = document.getElementById("almaic-advertising-space");
    const adObject = new AlmaicAdvert();

    // Create a new script element
    var script = document.createElement('script');

    // Set the src attribute to the URL of the external JavaScript file
    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.8/signalr.min.js';

    script.onload = function () {
        console.log("4");
        // Define a JavaScript module for event tracking
        const EventTracker = (function () {
            // Initialize SignalR connection
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7700/hubs/adeventmetrics") // Adjust the URL as needed
                .configureLogging(signalR.LogLevel.Information)
                .build();

            // Function to send event to the server
            function sendEvent(eventType, eventData) {
                console.log("7");
                connection.invoke("SendAdvertEvent", eventType, eventData).then(() => { console.log('SendAdvertEventFired'); }).catch(err => {
                    console.error(err.toString());
                });
            }

            // Function to attach event listeners to ad elements
            function attachEventListenersToAds() {
                const adElements = document.getElementsByClassName('ad'); // Adjust selector as needed
                for (let i = 0; i < adElements.length; i++) {
                    adElements[i].addEventListener('click', function (event) {
                        sendEvent('adClick', { adId: event.target.id }); // Send ad click event
                        console.log("SENT EVENT: " + "adClick" + " " + event.target.id)
                    });

                    // Add other event listeners as needed (e.g., impression, hover, etc.)
                    // Example: adElements[i].addEventListener('impression', function(event) { ... });
                }
                let hoverStartTime;
                adObject.addEventListener('mouseenter', (event) => {
                    hoverStartTime = Date.now();
                });
                adObject.addEventListener("mouseleave", (event) => {
                    const hoverEndTime = Date.now();
                    const hoverDuration = hoverEndTime - hoverStartTime;
                    const adId = event.target.id;
                    sendEvent("MouseHoverEvent", hoverDuration);
                });
                console.log("6");
            }

            // Initialize the event tracker
            function init() {
                console.log("5");
                // Start the SignalR connection
                connection.start().then(() => {
                    console.log("Connection established");
                    attachEventListenersToAds();
                    connection.invoke("SendAdvertEvent", "adClick", { adId: 1 }).then(() => { console.log('SendAdvertEventFired'); }).catch(err => {
                        console.error(err.toString());
                    });
                }).catch(err => {
                    console.error(err.toString());
                });
            }

            // Expose the init function publicly
            return {
                init: init
            };
        })();

        EventTracker.init();
    }


    const adProviderService = "https://localhost:7500"


    function injectHtmlIntoPage(html) {
        adObject.innerHTML = html;
        adContainer.appendChild(adObject);
    }

    fetch(adProviderService)
        .then(response => {
            if (!response.ok) {
                console.log(response.statusText);
            }
            console.log("Received: " + response.text)
            return response.text();
        })
        .then(html => {

            injectHtmlIntoPage(html);
            document.head.appendChild(script)
        })
        .catch(error => {
            console.error(error.message);
        });





}


