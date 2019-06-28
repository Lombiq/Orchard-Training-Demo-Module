// The main.js file is the entry point of a Vue.js app. From now on the Vue.js-specific code won't be commented except
// if it is related to Orchard Core.

import Vue from 'vue';
import DemoComponent from './components/demo-component';

// Here is the initDemoApp function defined you've seen right before.
window.initDemoApp = function (options) {
    // The options will be added to the Vue config object. To make it injectable you can add an options.js which proxies
    // the options in a strongly typed object (see Assets/Apps/demo/options.js). 
    Vue.config.demoApp = options;
    
    new Vue({
        el: options.element,
        components: {
            'demo-component': DemoComponent
        }
    });
};

// NEXT STATION: Assets/Apps/demo/components/demo-component.js