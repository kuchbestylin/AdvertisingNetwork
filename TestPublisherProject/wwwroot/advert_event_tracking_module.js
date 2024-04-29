
function initAdvertisingJs() {
    class AlmaicAdvert extends HTMLElement {
        constructor() {
            super();
        }
    }

    customElements.define("almaic-prologue", AlmaicAdvert);
    const adContainer = document.getElementById("almaic-advertising-space");
    const adObject = new AlmaicAdvert();

    var script = document.createElement('script');
    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.8/signalr.min.js';
    script.onload = function () {
        const EventTracker = (function () {

            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7700/hubs/adeventmetrics")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            function sendEvent(eventType, eventData) {
                console.log("7");
                connection.invoke("SendAdvertEvent", eventType, eventData).then(() => { console.log('SendAdvertEventFired'); }).catch(err => {
                    console.error(err.toString());
                });
            }


            function attachEventListenersToAds() {
                const adElements = document.getElementsByClassName('ad');
                for (let i = 0; i < adElements.length; i++) {
                    adElements[i].addEventListener('click', function (event) {
                        sendEvent('adClick', { adId: event.target.id });
                        console.log("SENT EVENT: " + "adClick" + " " + event.target.id)
                    });
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
            }


            function init() {
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


