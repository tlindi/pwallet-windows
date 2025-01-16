export function connectToSse(url, dotNetInstance) {
    const eventSource = new EventSource(url);

    eventSource.onmessage = (event) => {
        dotNetInstance.invokeMethodAsync('OnSseMessageReceived', event.data);
    };

    eventSource.onerror = (error) => {
        eventSource.close();
    };

    eventSource.onopen = () => {
    };
}