window.Observer = {
    observer: null,
    Initialize: function (component, observerTargetId) {
        this.observer = new IntersectionObserver(e => {
            console.log(e);
            if (e[0].isIntersecting) {
                component.invokeMethodAsync('OnIntersection');
            }
        });

        let element = document.getElementById(observerTargetId);
        if (element == null) throw new Error("The observable target was not found");
        this.observer.observe(element);
    }
};