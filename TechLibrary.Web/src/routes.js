import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const Home = () => import(/* webpackChunkName: "Home" */ './components/Home.vue');
const Book = () => import(/* webpackChunkName: "Book" */ './components/Book.vue');
const NewBook = () => import(/* webpackChunkName: "NewBook" */ './components/NewBook.vue');

const router = new VueRouter({
  routes: [
        {
            name: 'book_view',
            path: '/book/:id',
            component: Book,
            props: true
        },
        {
            name: 'newBook',
            path: '/create',
            component: NewBook,
            props: true
        },

        // define the / route last so that it is matched as the lowest priority 
        // (otherwise all the above routes would match it)
        { path: '/', component: Home }
  ]
});

export default router;