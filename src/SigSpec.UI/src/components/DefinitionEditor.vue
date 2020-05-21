<template>
  <div>
    <div>
      <div class="body-param" data-param-name="body" data-param-in="body">
        <textarea class="body-param__text" @change="$emit('input', text)" v-model="text"></textarea>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Definition } from '../models/Spec';
interface Dictionary<T> {
    [Key: string]: T;
}
export default Vue.extend({
    props: {
        definition: {
            type: Object as () => Definition
        }
    },
    data() {
        return {
            text: ''
        };
    },
    created() {
        const values: Dictionary<string> = {};
        for (const key in this.definition.properties) {
            values[
                this.definition.properties[key].name
            ] = this.definition.properties[key].types[
                this.definition.properties[key].types.length - 1
            ];
        }
        this.text = JSON.stringify(values, null, 2);
        this.$emit('input', this.text);
    }
});
</script>

<style scoped></style>
