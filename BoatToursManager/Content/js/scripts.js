(function () {
    'use strict'
    window.addEventListener('load', function () {
        addBootstrapValidation()

        $('.payment-form .nav-link').click(function (event) {
            var selectedType = $(event.target).data('type')
            $('#payment-type').val(selectedType)

            if (selectedType === 'credit-card') {
                // Validate the fields only if credit card
                // is selected as payment option
                $('.payment-form').removeAttr('novalidate')
            } else {
                $('.payment-form').attr('novalidate', 'novalidate')
            }
        })

        $('#print-receipt').click(function (event) {
            window.print()
        })

        $('#cart_button').click(function (event) {
            console.log("show clicked");
            var _token = document.getElementById("_token").value;
            $.ajax({
                url:'./show_basket',
                type: 'post',
                data: {
                    showcart: "cart",
                    '_token': _token,

                },
                success: function (response) {
                    console.log(response);
                    document.getElementById("mycart").innerHTML = response;
                    $("#mycart").slideToggle("slow");
                }
            });

        })


    }, false)

    /**
     * Disable form submissions if there are invalid fields
     */
    function addBootstrapValidation() {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation')
        // Loop over them and prevent submission
        Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault()
                    event.stopPropagation()
                }
                form.classList.add('was-validated')
            }, false)
        })
    }
})()

// function show_cart() {
//     var _token = document.getElementById("_token").value;
//     $.ajax({
//         type: 'post',
//         data: {
//             showcart: "cart",
//             '_token': _token,
//
//         },
//         success: function (response) {
//             document.getElementById("mycart").innerHTML = response;
//             $("#mycart").slideToggle();
//         }
//     });
//
// }


