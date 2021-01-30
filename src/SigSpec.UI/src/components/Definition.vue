<template>
  <div id="model-ApiResponse" @click="open = !open" class="model-container">
    <span class="models-jump-to-path"></span>
    <span class="model-box">
      <div class="model-box">
        <span class="model">
          <span class>
            <span style="cursor: pointer;">
              <span class="model-title">
                <span class="model-title__text">{{ definition.name }}</span>
              </span>
            </span>
            <span style="cursor: pointer;">
              <span class="model-toggle"></span>
            </span>
            <span class="brace-open object">{</span>
            <span class="inner-object" v-if="open">
              <table class="model">
                <tbody>
                  <tr :key="prop.name" v-for="prop in definition.properties" class="false">
                    <td style="vertical-align: top; padding-right: 0.2em;">{{ prop.name }}</td>
                    <td style="vertical-align: top;">
                      <span class="model">
                        <span class="prop">
                          <span class="prop-type">{{ propertyType(prop.types) }}</span>
                          <!--  <span class="prop-format">{{propertyType(prop.type)}}</span> -->
                        </span>
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </span>
            <span class="brace-close">}</span>
          </span>
        </span>
      </div>
    </span>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    props: {
        definition: Object
    },
    data() {
        return {
            open: false
        };
    },
    methods: {
        propertyType(prop: string[]) {
            const type = prop[prop.length - 1];
            const nullable = prop[0] == 'null';

            return `${type}${nullable ? '?' : ''}`;
        }
    }
});
</script>

<style scoped></style>
