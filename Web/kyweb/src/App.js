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
  const [activePath, setActivePath] = useState(location.pathname);

  return(
    <nav className = "header">
      <ul>
        <motion.li key = "home"
                   onClick={()=>{setActivePath("home")}}
                   initial = {false}
                   animate = {{backgroundColor: "home" === activePath ? "#eee" : "#eee0"}}>
          <Link to="/">홈{"home" === activePath ? (<motion.span className = "slider" id = "underline" layoutId = "underline"/>) : null}</Link>
        </motion.li>
        <motion.li key = "download" 
                   onClick={()=>{setActivePath("download")}}
                   initial = {false}
                   animate = {{backgroundColor: "download" === activePath ? "#eee" : "#eee0"}}>
          <Link to="/download">다운로드{"download" === activePath ? (<motion.span className = "slider" id = "underline" layoutId = "underline"/>) : null}</Link>
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
