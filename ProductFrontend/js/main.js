const apiUrl = 'https://localhost:7083/api/products'; // Thay đúng port của bạn

document.addEventListener('DOMContentLoaded', function () {
    fetchProducts();
    document.getElementById('btnAdd').addEventListener('click', addProduct);
    document.getElementById('btnUpdate').addEventListener('click', updateProductFromForm);
    document.getElementById('btnReset').addEventListener('click', resetForm);
})

function fetchProducts() {
    fetch(apiUrl)
        .then(handleResponse)
        .then(data => displayProducts(data))
        .catch(error => console.error('Fetch error:', error.message));
}

function handleResponse(response) {
    if (!response.ok) throw new Error('Network response was not ok');
    return response.json();
}

function displayProducts(products) {
    const bookList = document.getElementById('bookList');
    bookList.innerHTML = '';
    products.forEach(product => {
        bookList.innerHTML += createProductRow(product);
    });
}

function createProductRow(product) {
    return `
        <tr>
            <td>${product.id}</td>
            <td>${product.name}</td>
            <td>${product.price}</td>
            <td>${product.description}</td>
            <td class="text-end">
                <button class="btn btn-danger btn-sm delete-btn" data-id="${product.id}">Delete</button>
                <button class="btn btn-warning btn-sm edit-btn" data-id="${product.id}">Edit</button>
                <button class="btn btn-primary btn-sm view-btn" data-id="${product.id}">View</button>
            </td>
        </tr>
    `;
}

function addProduct() {
    const productData = {
        name: document.getElementById('bookName').value,
        price: document.getElementById('price').value,
        description: document.getElementById('description').value,
    };

    fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(productData),
    })
        .then(handleResponse)
        .then(data => {
            console.log('Product added:', data);
            fetchProducts();
        })
        .catch(error => console.error('Error:', error));
}

function viewProduct(id) {
    fetch(`${apiUrl}/${id}`)
        .then(handleResponse)
        .then(product => {
            alert(`ID: ${product.id}\nTên: ${product.name}\nGiá: ${product.price}\nMô tả: ${product.description}`);
        })
        .catch(error => console.error('Error:', error));
}

// Xóa sản phẩm
function deleteProduct(id) {
    if (!confirm('Bạn có chắc muốn xóa sản phẩm này?')) return;

    fetch(`${apiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.status === 204) {
                console.log('Product deleted successfully.');
                fetchProducts(); // Tải lại danh sách
            } else {
                console.error('Failed to delete product.');
            }
        })
        .catch(error => console.error('Error:', error));
}

// Sửa sản phẩm
function editProduct(id) {
    fetch(`${apiUrl}/${id}`)
        .then(handleResponse)
        .then(product => {
            document.getElementById('productId').value = product.id;
            document.getElementById('bookName').value = product.name;
            document.getElementById('price').value = product.price;
            document.getElementById('description').value = product.description;
        })
        .catch(error => console.error('Error:', error));
}

function resetForm() {
    document.getElementById('productId').value = 0;
    document.getElementById('bookName').value = '';
    document.getElementById('price').value = '';
    document.getElementById('description').value = '';
}

function updateProductFromForm() {
    const id = document.getElementById('productId').value;
    if (id === '0') {
        alert('Vui lòng chọn sản phẩm cần sửa (bấm Edit) trước khi Cập nhật.');
        return;
    }

    const updatedProduct = {
        id: parseInt(id),
        name: document.getElementById('bookName').value,
        price: parseFloat(document.getElementById('price').value),
        description: document.getElementById('description').value,
    };

    fetch(`${apiUrl}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedProduct),
    })
        .then(response => {
            if (response.status === 204) {
                console.log('Product updated successfully.');
                fetchProducts();
                resetForm();
            } else {
                console.error('Failed to update product.');
            }
        })
        .catch(error => console.error('Error:', error));
}

document.getElementById('bookList').addEventListener('click', function (e) {
    const id = e.target.getAttribute('data-id');
    if (!id) return;

    if (e.target.classList.contains('delete-btn')) {
        deleteProduct(id);
    } else if (e.target.classList.contains('edit-btn')) {
        editProduct(id);
    } else if (e.target.classList.contains('view-btn')) {
        viewProduct(id);
    }
});