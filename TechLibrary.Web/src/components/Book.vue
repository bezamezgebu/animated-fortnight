<template>
    <b-container fluid class="bv-example-row pt-5">
        <b-row>
            <b-col md="6" offset-md="3">
                <b-card v-if="book" :img-src="book.thumbnailUrl" img-alt="thumbnailUrl" img-top tag="article" style="max-width: 30rem;" class="mb-2 p-4">
                    <h4 v-if="!editMode">{{ book.title }}</h4>
                    <b-form-input v-if="editMode" v-model="book.title"></b-form-input>
                    <hr />
                    <p class="text-uppercase" v-if="!editMode">{{ book.isbn }}</p>
                    <b-form-input v-if="editMode" v-model="book.isbn"></b-form-input>

                    <b-card-text v-if="!editMode">{{ book.descr }}</b-card-text>

                    <b-card-text v-if="editMode">
                        <b-textarea v-model="book.descr" id="description" placeholder="description" rows="3" max-rows="100"></b-textarea>
                    </b-card-text>

                    <b-form-input v-if="editMode" v-model="book.thumbnailUrl"></b-form-input>

                    <b-row>
                        <b-col>
                            <b-button to="/" v-if="!editMode" variant="primary">Back</b-button>
                        </b-col>
                        <b-col class="text-right">
                            <b-button @click="toggleEdit" v-if="!editMode" variant="outline-primary">Edit</b-button>
                            <b-button @click="cancelEdit" v-if="editMode" variant="outline-primary">Cancel</b-button>
                            <b-button @click="saveEdit" v-if="editMode" variant="outline-primary">Save changes</b-button>
                        </b-col>
                    </b-row>
                </b-card>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
    import axios from "axios";

    export default {
        name: "Book",
        props: ["id"],
        data: () => ({
            book: null,
            editMode: false,
            item: {}
        }),

        mounted() {
            this.getDetail();
        },
        methods: {
            getDetail() {
                axios.get(`https://localhost:5001/books/${this.id}`).then(response => {
                    this.book = response.data;
                });
            },
            toggleEdit() {
                this.editMode = true;
            },
            cancelEdit() {
                this.editMode = false;
            },
            saveEdit() {
                axios
                    .put(`https://localhost:5001/books/${this.id}`, {
                        isbn: this.book.isbn,
                        title: this.book.title,
                        descr: this.book.descr,
                        thumbnailUrl: this.book.thumbnailUrl
                    })
                    .then(response => {
                        this.editMode = false;
                        this.book = response.data;
                    })
                    .then(this.getDetail())
                    .then(window.location.reload());
            }
        }
    };
</script>