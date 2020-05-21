import Vue from 'vue';
import App from './App.vue';

import 'swagger-ui/dist/swagger-ui.css';

Vue.config.productionTip = false;

new Vue({
  render: (h) => h(App),
}).$mount('#app');
