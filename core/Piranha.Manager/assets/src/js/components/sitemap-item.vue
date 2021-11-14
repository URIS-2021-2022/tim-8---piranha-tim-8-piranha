<template>
    <ul>
        <li class="dd-item" :class="{ expanded: item.isExpanded || item.items.length === 0 }" :data-id="item.id">
            <div class="sitemap-item" :class="{ dimmed: item.isUnpublished || item.isScheduled }">
                <div class="handle dd-handle"><em class="fas fa-ellipsis-v"></em></div>
                <div class="link">
                    <span class="actions">
                        <a v-if="item.items.length > 0 && item.isExpanded" v-on:click.prevent="toggleItem(item)" class="expand" href="#"><em class="fas fa-minus"></em></a>
                        <a v-if="item.items.length > 0 && !item.isExpanded" v-on:click.prevent="toggleItem(item)" class="expand" href="#"><em class="fas fa-plus"></em></a>
                    </span>
                    <a v-if="piranha.permissions.pages.edit" :href="piranha.baseUrl + item.editUrl + item.id">
                        <span>{{ item.title }}</span>
                        <span v-if="item.isRestricted" class="icon-restricted text-secondary small"><em class="fas fa-lock"></em></span>
                        <span v-if="item.status" class="badge badge-info">{{ item.status }}</span>
                        <span v-if="item.isScheduled" class="badge badge-info">{{ piranha.resources.texts.scheduled }}</span>
                        <span v-if="item.isCopy" class="badge badge-warning">{{ piranha.resources.texts.copy }}</span>
                    </a>
                    <span v-else class="title">
                        <span>{{ item.title }}</span>
                        <span v-if="item.isRestricted" class="icon-restricted text-secondary small"><em class="fas fa-lock"></em></span>
                        <span v-if="item.status" class="badge badge-info">{{ item.status }}</span>
                        <span v-if="item.isScheduled" class="badge badge-info">{{ piranha.resources.texts.scheduled }}</span>
                        <span v-if="item.isCopy" class="badge badge-warning">{{ piranha.resources.texts.copy }}</span>
                    </span>
                </div>
                <div class="type d-none d-md-block">{{ item.typeName }}</div>
                <div class="date d-none d-lg-block">{{ item.published }}</div>
                <div class="actions">
                    <a v-if="piranha.permissions.pages.add" href="#" v-on:click.prevent="piranha.pagelist.add(item.siteId, item.id, true)"><em class="fas fa-angle-down"></em></a>
                    <a v-if="piranha.permissions.pages.add" href="#" v-on:click.prevent="piranha.pagelist.add(item.siteId, item.id, false)"><em class="fas fa-angle-right"></em></a>
                    <a v-if="piranha.permissions.pages.delete && item.items.length === 0" v-on:click.prevent="piranha.pagelist.remove(item.id)" class="danger" href="#"><em class="fas fa-trash"></em></a>
                </div>
            </div>
            <ol v-if="item.items.length > 0" class="dd-list">
                <sitemap-item v-for="child in item.items" v-bind:key="child.id" v-bind:item="child">
                </sitemap-item>
            </ol>
        </li>
    </ul>
</template>

<script>
    export default {
        props: ["item"],
        methods: {
            toggleItem: function (item) {
                item.isExpanded = !item.isExpanded;
            }
        }
    }
</script>
