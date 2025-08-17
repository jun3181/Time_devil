import logo from './logo.svg';
import './App.css';
import { AnimatePresence, motion } from "motion/react";
import { BrowserRouter, Routes, Route, Link, useLocation } from "react-router-dom";
import { useState } from 'react';
import HomePage from "./Homepage.jsx";
import DownloadPage from "./Downloadpage.jsx";


const Page = ({ children }) => {
  return( 
    <motion.div
      className = "page"
      initial={{x:"100%"}}
      animate={{x:0}}
      exit={{x:"-100%"}}
      transition={{duration:0.5}}
    >
      {children}</motion.div>
  )
}

function Header() {
  const location = useLocation();

  return(
    <nav className = "header">
      <ul>
        <motion.li key = "home"
                   initial = {false}
                   animate = {{backgroundColor: "/" === location.pathname ? "#eee" : "#eee0"}}>
          <Link to="/">홈</Link>{"/" === location.pathname ? (<motion.span className = "slider" layoutId = "slider"/>) : null}
        </motion.li>
        <motion.li key = "download" 
                   initial = {false}
                   animate = {{backgroundColor: "/download" === location.pathname ? "#eee" : "#eee0"}}>
          <Link to="/download">다운로드</Link>{"/download" === location.pathname ? (<motion.span className = "slider" layoutId = "slider"/>) : null}
        </motion.li>

      </ul>
    </nav>
  )
}




function AnimatedRoutes() {
  const location = useLocation();
  return (
    <>
      <Header ></Header>
      <AnimatePresence>
        <Routes location={location} key={location.pathname}>
          <Route path="/" element={<Page><HomePage /></Page>} />
          <Route path="/download" element={<Page><DownloadPage /></Page>} />
        </Routes>
      </AnimatePresence>
    </>
  )
}


function App() {
  return (
    <div className="App">
      
      
      <BrowserRouter>
        <AnimatedRoutes />
      </BrowserRouter>


    </div>
  );
}

export default App;
