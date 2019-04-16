import Vue from 'vue';

class DemoOptions {
    static get antiForgeryToken() {
        return Vue.config.demoApp.antiForgeryToken;
    }

    static get text() {
        return Vue.config.demoApp.text;
    }
}

export default DemoOptions;