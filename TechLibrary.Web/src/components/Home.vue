<template>
    <div class="home">
        <h1>{{ msg }}</h1>

        <b-table 
                 id="table"
                 ref="table"
                 striped
                 hover
                 :items="updateBooks"
                 :fields="fields"
                 responsive="sm"
         >
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>

        <div id="pager-buttons">
            <button v-on:click="startItem += 10">Next page</button>
            <p>startItem value is {{ startItem }}.</p>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: {
            msg: String
        },
        data: () => ({
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            books: [],
            perPage: 10,
            startItem: 1
        }),

        watch: {
            // re-fetch books data when startItem changes
            startItem: function () {
                this.books = this.updateBooks();
                this.$refs.table.refresh()
            }
        },

        methods: {
            dataContext(ctx, callback) {
                axios.get(`https://localhost:5001/books?start=${this.startItem}&count=${this.perPage}`)
                    .then(response => {
                        callback(response.data);
                    });
            },

            updateBooks() {
                    const promise = axios.get(`https://localhost:5001/books?start=${this.startItem}&count=${this.perPage}`);
               
                    return promise.then(response => {
                    const data = response.data
                    // Must return an array of items or an empty array if an error occurred
                    return data || []
                })
            },
        },

        created() {
            this.books = this.updateBooks();
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
