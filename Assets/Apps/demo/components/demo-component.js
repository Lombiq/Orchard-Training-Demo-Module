// Here this is a very simple Vue.js component. As you can see this is not a .vue file. The reason for this is that we
// can keep our templates in shapes so those can be overridden and localized by Orchard Core. The styling for this is
// also separated from the Vue app as mentioned before.

import options from '../options';

export default {
    name: 'demo-component',
    template: '#demo-component',

    data() {
        return {
            showText: false,
            text: options.text
        };
    }
};

// NEXT STATION: Views/VueComponents/Demo.Component.cshtml