/**
 * Ask confirmation before deleting a product
 */
$('.delete-product-form').submit(function (event) {
  if (!window.confirm('Are you sure you want to delete the product?')) {
    event.preventDefault()
    return false
  }
})
