// ============================================
// Retro Game Store — validation.js
// Client-side form validation
// ============================================

document.addEventListener('DOMContentLoaded', function () {

    // ── Helper: show error message below a field ──
    function showError(input, message) {
        clearError(input);
        input.classList.add('is-invalid');
        var errorSpan = document.createElement('span');
        errorSpan.className = 'text-danger small validation-error';
        errorSpan.textContent = message;
        input.parentNode.appendChild(errorSpan);
    }

    // ── Helper: clear error message from a field ──
    function clearError(input) {
        input.classList.remove('is-invalid');
        var existing = input.parentNode.querySelector('.validation-error');
        if (existing) {
            existing.remove();
        }
    }

    // ── Helper: check if a string is a valid email format ──
    function isValidEmail(email) {
        var pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return pattern.test(email);
    }


    // ═══════════════════════════════════════════
    // 1. LOGIN FORM VALIDATION
    // ═══════════════════════════════════════════
    var loginForm = document.querySelector('form[action*="/Account/Login"]');
    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            var isValid = true;

            var emailInput = loginForm.querySelector('input[name="Email"], #Email');
            var passwordInput = loginForm.querySelector('input[name="Password"], #Password');

            // Validate email
            if (emailInput) {
                var email = emailInput.value.trim();
                if (email === '') {
                    showError(emailInput, 'Email is required.');
                    isValid = false;
                } else if (!isValidEmail(email)) {
                    showError(emailInput, 'Please enter a valid email address.');
                    isValid = false;
                } else {
                    clearError(emailInput);
                }
            }

            // Validate password
            if (passwordInput) {
                var password = passwordInput.value.trim();
                if (password === '') {
                    showError(passwordInput, 'Password is required.');
                    isValid = false;
                } else if (password.length < 4) {
                    showError(passwordInput, 'Password must be at least 4 characters.');
                    isValid = false;
                } else {
                    clearError(passwordInput);
                }
            }

            if (!isValid) {
                e.preventDefault();
            }
        });

        // Clear errors on input (real-time feedback)
        var loginInputs = loginForm.querySelectorAll('input');
        loginInputs.forEach(function (input) {
            input.addEventListener('input', function () {
                clearError(input);
            });
        });
    }


    // ═══════════════════════════════════════════
    // 2. GAME CREATE / EDIT FORM VALIDATION
    // ═══════════════════════════════════════════
    var gameForm = document.querySelector('form[action*="/Games/Create"], form[action*="/Games/Edit"]');
    if (gameForm) {
        gameForm.addEventListener('submit', function (e) {
            var isValid = true;

            // Title — required, min 2 chars
            var titleInput = gameForm.querySelector('#Title');
            if (titleInput) {
                var title = titleInput.value.trim();
                if (title === '') {
                    showError(titleInput, 'Game title is required.');
                    isValid = false;
                } else if (title.length < 2) {
                    showError(titleInput, 'Title must be at least 2 characters.');
                    isValid = false;
                } else {
                    clearError(titleInput);
                }
            }

            // Price — required, must be a positive number
            var priceInput = gameForm.querySelector('#Price');
            if (priceInput) {
                var price = parseFloat(priceInput.value);
                if (priceInput.value.trim() === '') {
                    showError(priceInput, 'Price is required.');
                    isValid = false;
                } else if (isNaN(price) || price <= 0) {
                    showError(priceInput, 'Price must be a positive number.');
                    isValid = false;
                } else if (price > 9999) {
                    showError(priceInput, 'Price cannot exceed $9,999.');
                    isValid = false;
                } else {
                    clearError(priceInput);
                }
            }

            // Stock — required, must be 0 or more
            var stockInput = gameForm.querySelector('#Stock');
            if (stockInput) {
                var stock = parseInt(stockInput.value);
                if (stockInput.value.trim() === '') {
                    showError(stockInput, 'Stock is required.');
                    isValid = false;
                } else if (isNaN(stock) || stock < 0) {
                    showError(stockInput, 'Stock must be 0 or more.');
                    isValid = false;
                } else {
                    clearError(stockInput);
                }
            }

            // Release Year — required, must be a valid year
            var yearInput = gameForm.querySelector('#ReleaseYear');
            if (yearInput) {
                var year = parseInt(yearInput.value);
                var currentYear = new Date().getFullYear();
                if (yearInput.value.trim() === '') {
                    showError(yearInput, 'Release year is required.');
                    isValid = false;
                } else if (isNaN(year) || year < 1970 || year > currentYear) {
                    showError(yearInput, 'Year must be between 1970 and ' + currentYear + '.');
                    isValid = false;
                } else {
                    clearError(yearInput);
                }
            }

            // Low Stock Threshold — optional but if filled, must be valid
            var thresholdInput = gameForm.querySelector('#LowStockThreshold');
            if (thresholdInput && thresholdInput.value.trim() !== '') {
                var threshold = parseInt(thresholdInput.value);
                if (isNaN(threshold) || threshold < 0) {
                    showError(thresholdInput, 'Threshold must be 0 or more.');
                    isValid = false;
                } else {
                    clearError(thresholdInput);
                }
            }

            if (!isValid) {
                e.preventDefault();
                // Scroll to the first error
                var firstError = gameForm.querySelector('.is-invalid');
                if (firstError) {
                    firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    firstError.focus();
                }
            }
        });

        // Real-time: clear errors as user types
        var gameInputs = gameForm.querySelectorAll('input, select');
        gameInputs.forEach(function (input) {
            input.addEventListener('input', function () {
                clearError(input);
            });
        });
    }


    // ═══════════════════════════════════════════
    // 3. QUANTITY INPUT VALIDATION (Browse & Details pages)
    // ═══════════════════════════════════════════
    var qtyForms = document.querySelectorAll('form[action*="/Cart/AddToCart"]');
    qtyForms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            var qtyInput = form.querySelector('input[name="quantity"]');
            if (qtyInput) {
                var qty = parseInt(qtyInput.value);
                var max = parseInt(qtyInput.getAttribute('max')) || 99;

                if (isNaN(qty) || qty < 1) {
                    e.preventDefault();
                    showError(qtyInput, 'Quantity must be at least 1.');
                } else if (qty > max) {
                    e.preventDefault();
                    showError(qtyInput, 'Only ' + max + ' in stock.');
                } else {
                    clearError(qtyInput);
                }
            }
        });
    });

});
