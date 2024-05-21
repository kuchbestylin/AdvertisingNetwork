const adProviderService = "https://localhost:10000/bidding/getmatch/"
const hubProvider = "https://localhost:7009/hubs/adeventmetrics"
var currentScript = document.currentScript;
var srcAttribute = currentScript.getAttribute('src');
var fingerprintValue = currentScript.getAttribute('fingerprint');
var campaignId = 0;
window.onload = function () {
    const requestOptions = {
        method: 'GET'
    };

    var signalRScript = document.createElement('script');
    signalRScript.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.8/signalr.min.js';
    signalRScript.onload = function () {
        const EventTracker = (function () {

            const connection = new signalR.HubConnectionBuilder()
                .withUrl(hubProvider, {
                    skipNegotiation: true,
                    transport: signalR.HttpTransportType.WebSockets
                })
                .configureLogging(signalR.LogLevel.Information)
                .build();

            function sendEvent(name, count, id) {
                connection.invoke("SendAdvertEvent", name, count, id)
                    .then(() => { console.log('SendAdvertEventFired'); })
                    .catch(err => {
                        console.error(err.toString());
                    });
            }

            function attachEventListenersToAds() {
                const adLink = document.getElementById('advert-link-address');
                adLink.addEventListener('click', function (event) {
                    sendEvent('click', 1, campaignId);
                    console.log("SENT EVENT: " + "click" + " " + campaignId)
                });
                let hoverStartTime;
                const adObject = document.getElementById('almaic-advertising-space');
                adObject.addEventListener('mouseenter', (event) => {
                    hoverStartTime = Date.now();
                });
                adObject.addEventListener("mouseleave", (event) => {
                    const hoverEndTime = Date.now();
                    const hoverDuration = hoverEndTime - hoverStartTime;
                    sendEvent("hover", hoverDuration, campaignId);
                });
            }


            function init() {
                connection.start().then(() => {
                    console.log("Connection established");
                    attachEventListenersToAds();
                    connection.invoke("SendAdvertEvent", "show", 1, campaignId)
                        .then(() => { console.log('SendAdvertEventFired'); })
                        .catch(err => {
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

    function injectHtmlIntoPage(data) {
        const adContainer = document.getElementById("almaic-advertising-space");

        // Create advert container element
        const adObject = document.createElement('div');
        adObject.classList.add('advert-container');
        adObject.style.width = data.width;
        adObject.style.height = "auto";

        // Create link element for redirection
        const link = document.createElement('a');
        link.id = "advert-link-address";
        link.href = data.redirect;
        link.target = '_blank';

        // Create image element
        const image = document.createElement('img');
        image.src = data.imagelink;
        image.alt = 'Advertisement Image';
        image.style.width = '100%';
        image.style.height = 'auto';
        link.appendChild(image);

        // Create advert content
        const advertContent = document.createElement('div');
        advertContent.classList.add('advert-content');

        // Create heading
        const heading = document.createElement('h2');
        heading.textContent = data.heading;

        // Create description
        const description = document.createElement('p');
        description.textContent = data.description;

        // Append heading and description to advert content
        advertContent.appendChild(heading);
        advertContent.appendChild(description);

        // Append link and advert content to advert container
        adObject.appendChild(link);
        adObject.appendChild(advertContent);

        // Append advert container to ad container in the DOM
        adContainer.appendChild(adObject);
    }
    function createPopupAd(data) {
        // Create modal container
        const modal = document.createElement('div');
        modal.style.display = 'none'; // Hide modal by default
        modal.style.position = 'fixed';
        modal.style.zIndex = '9999';
        modal.style.left = '0';
        modal.style.top = '0';
        modal.style.width = '100%';
        modal.style.height = '100%';
        modal.style.overflow = 'hidden';
        modal.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';

        // Create modal content
        const modalContent = document.createElement('div');
        modalContent.id = "almaic-advertising-space";
        modalContent.style.backgroundColor = '#fff';
        modalContent.style.borderRadius = '5px';
        modalContent.style.margin = '3% auto';
        modalContent.style.padding = '20px';
        modalContent.style.maxWidth = '480px';

        // Create close button
        const closeButton = document.createElement('span');
        closeButton.innerHTML = '&times;';
        closeButton.style.color = '#aaa';
        closeButton.style.float = 'right';
        closeButton.style.fontSize = '28px';
        closeButton.style.fontWeight = 'bold';
        closeButton.style.cursor = 'pointer';

        // Append close button to modal content
        modalContent.appendChild(closeButton);

        // Create link element for redirection
        const link = document.createElement('a');
        link.id = "advert-link-address";
        link.href = data.redirect;
        link.target = '_blank';

        // Create image element
        const image = document.createElement('img');
        image.src = data.imagelink;
        image.alt = 'Advertisement Image';
        image.style.width = '100%';
        image.style.height = 'auto';
        link.appendChild(image);

        // Create advert content
        const advertContent = document.createElement('div');

        // Create heading
        const heading = document.createElement('h2');
        heading.textContent = data.heading;

        // Create description
        const description = document.createElement('p');
        description.textContent = data.description;

        // Append heading and description to advert content
        advertContent.appendChild(heading);
        advertContent.appendChild(description);

        // Append link and advert content to modal content
        modalContent.appendChild(link);
        modalContent.appendChild(advertContent);

        // Append modal content to modal container
        modal.appendChild(modalContent);

        // Append modal container to body
        document.body.appendChild(modal);

        // Show modal
        modal.style.display = 'block';

        // Close modal when close button is clicked
        closeButton.onclick = function () {
            modal.style.display = 'none';
        }

        // Close modal when clicked outside of it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    }

    //fetch(adProviderService)
    //    .then(response => {
    //        if (!response.ok) {
    //            console.log(response.statusText);
    //        }
    //        console.log("Received: " + response.text)
    //        return response.text();
    //    })
    //    .then(html => {

    //        injectHtmlIntoPage(html);
    //        document.head.appendChild(signalRScript)
    //    })
    //    .catch(error => {
    //        console.error(error.message);
    //    });


    fetch(adProviderService + fingerprintValue, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            campaignId = data.id;
            if (document.getElementById("almaic-advertising-space") != null) {
                createPopupAd(data);
            }
            else {
                createPopupAd(data);
            }
            document.head.appendChild(signalRScript)
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}