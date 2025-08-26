import React, { useState } from 'react';
import './App.css';

const initialTiles = [
  {
    src: '/logo192.png',
    title: '로고192',
    content: '로고192에 대한 설명입니다.',
    size: 'small',
    comments: [],
  },
  {
    src: '/logo512.png',
    title: '로고512',
    content: '로고512에 대한 설명입니다.',
    size: 'large',
    comments: [],
  },
  {
    src: '/favicon.ico',
    title: '파비콘',
    content: '파비콘에 대한 설명입니다.',
    size: 'medium',
    comments: [],
  },
];

function App() {
  const [tiles, setTiles] = useState(initialTiles);
  const [form, setForm] = useState({ title: '', src: '', content: '' });
  const [modalIdx, setModalIdx] = useState(null);
  const [comment, setComment] = useState('');
  const [nickname, setNickname] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };
  const handleNicknameChange = (e) => setNickname(e.target.value);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!form.title || !form.src) return;
    const sizes = ['small', 'medium', 'large'];
    const nextSize = sizes[tiles.length % sizes.length];
    setTiles([
      ...tiles,
      { ...form, size: nextSize, comments: [], nickname: nickname || '익명' }
    ]);
    setForm({ title: '', src: '', content: '' });
  };

  const openModal = (idx) => {
    setModalIdx(idx);
    setComment('');
  };
  const closeModal = () => setModalIdx(null);

  const handleCommentChange = (e) => setComment(e.target.value);
  const handleCommentSubmit = (e) => {
    e.preventDefault();
    if (!comment.trim()) return;
    setTiles(tiles => tiles.map((tile, idx) =>
      idx === modalIdx
        ? { ...tile, comments: [...tile.comments, { nickname: nickname || '익명', text: comment }] }
        : tile
    ));
    setComment('');
  };

  return (
    <div className="App">
      <h1>윈도우8 스타일 타일맵 게시판</h1>
      <div className="nickname-bar">
        <input
          type="text"
          value={nickname}
          onChange={handleNicknameChange}
          placeholder="닉네임을 입력하세요"
          style={{marginBottom:'12px',padding:'6px',borderRadius:'6px',border:'1px solid #ccc'}}
        />
      </div>
      <form className="tile-form" onSubmit={handleSubmit}>
        <input
          type="text"
          name="title"
          placeholder="제목"
          value={form.title}
          onChange={handleChange}
          required
        />
        <input
          type="text"
          name="src"
          placeholder="이미지 URL 또는 경로"
          value={form.src}
          onChange={handleChange}
          required
        />
        <textarea
          name="content"
          placeholder="내용"
          value={form.content}
          onChange={handleChange}
          rows={2}
        />
        <button type="submit">게시글 추가</button>
      </form>
      <div className="tilemap-board">
        {tiles.map((tile, idx) => (
          <div className={`tile ${tile.size}`} key={idx} onClick={() => openModal(idx)} style={{cursor:'pointer'}}>
            <img src={tile.src} alt={tile.title} />
            <div className="tile-title">{tile.title}</div>
          </div>
        ))}
      </div>
      {modalIdx !== null && (
        <div className="modal-overlay" onClick={closeModal}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{tiles[modalIdx].title}</h2>
            <div style={{fontSize:'0.95rem',color:'#888',marginBottom:'8px'}}>작성자: {tiles[modalIdx].nickname || '익명'}</div>
            <img src={tiles[modalIdx].src} alt={tiles[modalIdx].title} style={{width:'100%',borderRadius:'12px'}} />
            <p>{tiles[modalIdx].content}</p>
            <div className="comments">
              <h3>댓글</h3>
              <ul>
                {tiles[modalIdx].comments.map((c, i) => (
                  <li key={i}><b>{c.nickname}:</b> {c.text}</li>
                ))}
              </ul>
              <form onSubmit={handleCommentSubmit} className="comment-form">
                <input
                  type="text"
                  value={comment}
                  onChange={handleCommentChange}
                  placeholder="댓글 입력"
                  required
                />
                <button type="submit">등록</button>
              </form>
            </div>
            <button className="close-btn" onClick={closeModal}>닫기</button>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
