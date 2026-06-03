// ============================================
// Retro Game Store — site.js
// Global UI interactions & DOM manipulation
// ============================================

document.addEventListener('DOMContentLoaded', function () {

    // ── 1. Auto-dismiss alert messages after 4 seconds ──
    var alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        }, 4000);
    });


    // ── 2. Delete confirmation dialogs ──
    // Replaces inline onsubmit="return confirm(...)" on delete forms
    var deleteForms = document.querySelectorAll('.delete-form');
    deleteForms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            var gameName = form.getAttribute('data-game-title') || 'this item';
            var confirmed = confirm('Are you sure you want to delete "' + gameName + '"?');
            if (!confirmed) {
                e.preventDefault();
            }
        });
    });


    // ── 3. Cart quantity controls (+/− buttons) ──
    // Uses event delegation on the cart container
    var cartContainer = document.querySelector('.cart-items');
    if (cartContainer) {
        cartContainer.addEventListener('click', function (e) {
            var btn = e.target.closest('.qty-btn');
            if (!btn) return;

            var form = btn.closest('form');
            var input = form.querySelector('.qty-input');
            var currentVal = parseInt(input.value) || 1;

            if (btn.classList.contains('qty-decrease')) {
                input.value = Math.max(1, currentVal - 1);
            } else if (btn.classList.contains('qty-increase')) {
                input.value = currentVal + 1;
            }

            // Auto-submit the form to update the cart
            form.submit();
        });

        // Also submit when quantity input is changed manually
        var qtyInputs = cartContainer.querySelectorAll('.qty-input');
        qtyInputs.forEach(function (input) {
            input.addEventListener('change', function () {
                input.closest('form').submit();
            });
        });
    }


    // ── 4. Game Detail page — quantity controls ──
    var detailQtyInput = document.getElementById('detailQty');
    if (detailQtyInput) {
        var maxStock = parseInt(detailQtyInput.getAttribute('max')) || 99;

        var detailBtns = detailQtyInput.closest('.qty-control').querySelectorAll('.qty-btn');
        detailBtns.forEach(function (btn) {
            btn.addEventListener('click', function () {
                var currentVal = parseInt(detailQtyInput.value) || 1;
                if (btn.classList.contains('qty-decrease')) {
                    detailQtyInput.value = Math.max(1, currentVal - 1);
                } else if (btn.classList.contains('qty-increase')) {
                    detailQtyInput.value = Math.min(maxStock, currentVal + 1);
                }
            });
        });
    }


    // ── 5. Image gallery — thumbnail switching ──
    var galleryThumbs = document.querySelectorAll('.gallery-thumb');
    var mainImage = document.getElementById('mainImage');
    if (mainImage && galleryThumbs.length > 0) {
        galleryThumbs.forEach(function (thumb) {
            thumb.addEventListener('click', function () {
                // Update main image
                mainImage.src = thumb.src;
                // Toggle active class
                galleryThumbs.forEach(function (t) {
                    t.classList.remove('active');
                });
                thumb.classList.add('active');
            });
        });
    }


    // ── 6. Navbar — highlight active link ──
    var currentPath = window.location.pathname.toLowerCase();
    var navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(function (link) {
        var href = link.getAttribute('href');
        if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
    });


    // ── 7. Smooth scroll to top on page load ──
    window.scrollTo({ top: 0, behavior: 'smooth' });

});
