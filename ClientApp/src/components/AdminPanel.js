import React, { useState } from 'react';
import axios from 'axios';

function AdminPanel() {
  const [productId, setProductId] = useState('');
  const [newPrice, setNewPrice] = useState('');
  const [message, setMessage] = useState('');

  const updatePrice = async (e) => {
    e.preventDefault();
    // Vulnerable: No auth, direct object reference
    try {
      await axios.put(`/api/product/update-price/${productId}?newPrice=${newPrice}`);
      setMessage('Price updated!');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  return (
    <div>
      <h2>Admin Panel</h2>
      <form onSubmit={updatePrice}>
        <input value={productId} onChange={e => setProductId(e.target.value)} placeholder="Product ID" />
        <input value={newPrice} onChange={e => setNewPrice(e.target.value)} placeholder="New Price" />
        <button type="submit">Update Price</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
    </div>
  );
}

export default AdminPanel; 