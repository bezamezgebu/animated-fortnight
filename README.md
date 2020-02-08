#### Notes

* Use a real (not mocked) mapper in unit tests so we can verify mapped responses.
* I have added another mapping profile ResponseToDomain to map BookRequest objects to the domain type Book.
* I have added a computed property to give a message "No More Results" when viewing books and searching for a book.
* I have added a native key handler "Enter" when searching for a book, so that the search box responds to the Enter key (as well as the Search button being clicked).
* I have added a newBook page on the UI for creating a new book.
* I added paging functionality for viewing search results (similar to the paging when viewing all books).
* I added unit tests for all service endpoints.