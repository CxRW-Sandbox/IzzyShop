import React, { useState, useEffect } from 'react';
import axios from 'axios';

function ProductDetail({ productId }) {
  const [product, setProduct] = useState(null);
  const [review, setReview] = useState('');
  const [message, setMessage] = useState('');
  const [metadata, setMetadata] = useState('');

  useEffect(() => {
    axios.get(`/api/product/${productId}`)
      .then(res => setProduct(res.data))
      .catch(() => setMessage('Error loading product.'));
  }, [productId]);

  const submitReview = async (e) => {
    e.preventDefault();
    // Vulnerable: No CSRF protection, no output encoding
    try {
      await axios.post('/api/product/review', {
        productId,
        content: review
      });
      setMessage('Review submitted!');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  const purchase = async () => {
    // Vulnerable: No auth, IDOR
    try {
      await axios.post('/api/product/purchase', {
        productId,
        quantity: 1
      });
      setMessage('Purchased!');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  const updateMetadata = async (e) => {
    e.preventDefault();
    try {
      await axios.post(`/api/product/add-metadata/${productId}`, metadata, { headers: { 'Content-Type': 'application/json' } });
      setMessage('Metadata updated!');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  if (!product) return <div>Loading...</div>;

  return (
    <div>
      <h3>{product.name}</h3>
      <div dangerouslySetInnerHTML={{ __html: product.description }} />
      <button onClick={purchase}>Buy Now</button>
      <h4>Reviews</h4>
      <ul>
        {(product.reviews || []).map((r, i) => (
          <li key={i} dangerouslySetInnerHTML={{ __html: r.content }} />
        ))}
      </ul>
      <form onSubmit={submitReview}>
        <textarea value={review} onChange={e => setReview(e.target.value)} placeholder="Write a review" />
        <button type="submit">Submit Review</button>
      </form>
      <h4>Metadata</h4>
      <div dangerouslySetInnerHTML={{ __html: product.metadata }} />
      <form onSubmit={updateMetadata}>
        <textarea value={metadata} onChange={e => setMetadata(e.target.value)} placeholder="Add metadata (XSS demo)" />
        <button type="submit">Update Metadata</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
    </div>
  );
}

export default ProductDetail; 