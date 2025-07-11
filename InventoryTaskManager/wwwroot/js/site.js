// Site-wide JavaScript for Inventory & Task Management System

$(document).ready(function() {
    // Initialize tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
    
    // Initialize popovers
    $('[data-bs-toggle="popover"]').popover();
    
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);
    
    // Form validation enhancements
    $('form').on('submit', function() {
        const submitBtn = $(this).find('button[type="submit"]');
        submitBtn.prop('disabled', true);
        submitBtn.find('i').removeClass().addClass('fas fa-spinner fa-spin');
        
        // Re-enable button after 3 seconds (in case of validation errors)
        setTimeout(function() {
            submitBtn.prop('disabled', false);
            submitBtn.find('i').removeClass().addClass('fas fa-save');
        }, 3000);
    });
    
    // Search functionality
    $('#searchInput').on('input', function() {
        const searchTerm = $(this).val().toLowerCase();
        filterTableRows(searchTerm);
    });
    
    // Filter table rows based on search term
    function filterTableRows(searchTerm) {
        $('tbody tr').each(function() {
            const rowText = $(this).text().toLowerCase();
            if (rowText.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }
    
    // Confirm delete actions
    $('.delete-confirm').on('click', function(e) {
        if (!confirm('Are you sure you want to delete this item?')) {
            e.preventDefault();
        }
    });
    
    // Auto-format currency inputs
    $('.currency-input').on('blur', function() {
        const value = parseFloat($(this).val());
        if (!isNaN(value)) {
            $(this).val(value.toFixed(2));
        }
    });
    
    // Auto-format number inputs
    $('.number-input').on('input', function() {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    
    // Dynamic stock level indicator
    $('.quantity-input').on('input', function() {
        const quantity = parseInt($(this).val()) || 0;
        const minLevel = parseInt($(this).data('min-level')) || 0;
        const indicator = $(this).siblings('.stock-indicator');
        
        if (quantity < minLevel) {
            indicator.removeClass('text-success').addClass('text-danger').text('Low Stock');
        } else {
            indicator.removeClass('text-danger').addClass('text-success').text('In Stock');
        }
    });
    
    // Quick action buttons
    $('.quick-action').on('click', function() {
        const action = $(this).data('action');
        const target = $(this).data('target');
        
        switch(action) {
            case 'refresh':
                location.reload();
                break;
            case 'print':
                window.print();
                break;
            case 'export':
                exportData();
                break;
        }
    });
    
    // Export functionality
    function exportData() {
        // Basic CSV export
        const table = $('table');
        if (table.length) {
            let csv = '';
            
            // Headers
            table.find('thead tr').each(function() {
                const row = [];
                $(this).find('th').each(function() {
                    row.push($(this).text().trim());
                });
                csv += row.join(',') + '\n';
            });
            
            // Data rows
            table.find('tbody tr:visible').each(function() {
                const row = [];
                $(this).find('td').each(function() {
                    row.push($(this).text().trim().replace(/,/g, ';'));
                });
                csv += row.join(',') + '\n';
            });
            
            // Download
            const blob = new Blob([csv], { type: 'text/csv' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'inventory-data.csv';
            a.click();
            window.URL.revokeObjectURL(url);
        }
    }
    
    // Real-time stock value calculation
    function calculateStockValue() {
        const price = parseFloat($('#Price').val()) || 0;
        const quantity = parseInt($('#Quantity').val()) || 0;
        const stockValue = price * quantity;
        
        $('.stock-value-display').text('$' + stockValue.toFixed(2));
    }
    
    // Bind stock value calculation to price and quantity inputs
    $('#Price, #Quantity').on('input', calculateStockValue);
    
    // Initialize stock value calculation on page load
    calculateStockValue();
});

// Global utility functions
window.InventoryManager = {
    // Show loading state
    showLoading: function(element) {
        $(element).addClass('loading');
    },
    
    // Hide loading state
    hideLoading: function(element) {
        $(element).removeClass('loading');
    },
    
    // Show notification
    showNotification: function(message, type = 'info') {
        const alertClass = 'alert-' + type;
        const alert = $('<div class="alert ' + alertClass + ' alert-dismissible fade show" role="alert">' +
            message +
            '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>' +
            '</div>');
        
        $('.container').first().prepend(alert);
        
        // Auto-hide after 5 seconds
        setTimeout(function() {
            alert.fadeOut('slow', function() {
                $(this).remove();
            });
        }, 5000);
    },
    
    // Format currency
    formatCurrency: function(amount) {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(amount);
    },
    
    // Format number
    formatNumber: function(number) {
        return new Intl.NumberFormat('en-US').format(number);
    }
};
