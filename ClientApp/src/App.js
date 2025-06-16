import React, { useState } from 'react';
import Login from './components/Login';
import Register from './components/Register';
import ProductList from './components/ProductList';
import ProductDetail from './components/ProductDetail';
import AdminPanel from './components/AdminPanel';
import FileUpload from './components/FileUpload';
import Order from './components/Order';

function App() {
  const [page, setPage] = useState('products');
  const [selectedProduct, setSelectedProduct] = useState(null);

  return (
    <div>
      <h1>IzzyShop (Vulnerable Demo)</h1>
      <nav>
        <button onClick={() => setPage('login')}>Login</button>
        <button onClick={() => setPage('register')}>Register</button>
        <button onClick={() => setPage('products')}>Products</button>
        <button onClick={() => setPage('admin')}>Admin</button>
        <button onClick={() => setPage('upload')}>File Upload</button>
        <button onClick={() => setPage('order')}>Order</button>
      </nav>
      <hr />
      {page === 'login' && <Login />}
      {page === 'register' && <Register />}
      {page === 'products' && <ProductList onSelect={setSelectedProduct} />}
      {page === 'admin' && <AdminPanel />}
      {page === 'upload' && <FileUpload />}
      {page === 'order' && <Order />}
      {selectedProduct && <ProductDetail productId={selectedProduct} />}
    </div>
  );
}

export default App; 