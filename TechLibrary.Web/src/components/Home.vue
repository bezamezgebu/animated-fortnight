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
            <button v-on:click="startItem -= 10" :disabled="isPrevButtonDisabled">Previous page</button>
            <button v-on:click="startItem += 10" :disabled="isNextButtonDisabled">Next page</button>
            <p>Displaying items {{ startItem }} - {{ startItem + books.length }}</p>
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
            startItem: 1,
        }),

        watch: {
            // re-fetch books data when startItem changes
            startItem: function () {
                this.$refs.table.refresh()
            }
        },

        computed: {
            isPrevButtonDisabled: function () {
                return this.startItem <= 10;
            },
            isNextButtonDisabled: function () {
                return this.books.length < 10;
            }
        },

        methods: {
            updateBooks() {
                    const promise = axios.get(`https://localhost:5001/books?start=${this.startItem}&count=${this.perPage}`);
               
                    return promise.then(response => {
                    const data = response.data

                    // TODO: figure out how to assign this variable by resolving the promise returned by calling this function
                    this.books = data || []
                    return data || []
                })
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
