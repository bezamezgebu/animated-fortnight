<template>
    <b-card>
        <b-form-input v-model="title" placeholder="title"></b-form-input>
        <b-form-input v-model="isbn" placeholder="isbn"></b-form-input>
        <b-form-input v-model="thumbnailUrl" placeholder="thumbnailUrl"></b-form-input>
        <b-textarea  v-model="descr" id="description" placeholder="description" rows="3" max-rows="100"></b-textarea>
        <b-row>
            <b-col>
                <b-button to="/" variant="primary">Back</b-button>
                <b-button @click="saveBook" variant="outline-primary">Save </b-button>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            isbn: "",
            title: "",
            descr: "",
            thumbnailUrl: ""
        }),
        methods: {
            saveBook() {
                axios.post(`https://localhost:5001/books/create`, {
                    isbn: this.isbn,
                    title: this.title,
                    descr: this.descr,
                    thumbnailUrl: this.thumbnailUrl
                })
                    .then(
                        this.$router.push("/")
                    )
            }
         }
    }
</script>

<style scoped>
</style>