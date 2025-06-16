import React, { useState, useEffect } from 'react';
import axios from 'axios';

function ProductList({ onSelect }) {
  const [products, setProducts] = useState([]);
  const [search, setSearch] = useState('');
  const [message, setMessage] = useState('');

  const fetchProducts = async (q) => {
    try {
      // Vulnerable: No output encoding, reflected XSS in message
      const res = await axios.get(`/api/product/search?query=${q || ''}`);
      setProducts(res.data);
      setMessage(q ? `Results for: ${q}` : '');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  useEffect(() => {
    fetchProducts('');
  }, []);

  return (
    <div>
      <h2>Products</h2>
      <form onSubmit={e => { e.preventDefault(); fetchProducts(search); }}>
        <input value={search} onChange={e => setSearch(e.target.value)} placeholder="Search" />
        <button type="submit">Search</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
      <ul>
        {products.map(p => (
          <li key={p.id}>
            <a href="#" onClick={() => onSelect(p.id)}>{p.name}</a> - ${p.price} <br />
            <span dangerouslySetInnerHTML={{ __html: p.description }} />
          </li>
        ))}
      </ul>
    </div>
  );
}

export default ProductList; 