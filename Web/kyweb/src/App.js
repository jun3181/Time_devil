import logo from './logo.svg';
import './App.css';
import { AnimatePresence, motion } from "motion/react";
import { BrowserRouter, Routes, Route, Link, useLocation } from "react-router-dom";


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
  return(
    <div>
      <Link to="/">홈</Link>  <Link to="/download">다운로드</Link>
    </div>
  )
}

function HomePage(){
  return(
    <div>
      <img src = {logo}></img>
      <h2>타임 데빌</h2>
      <h3>게임 설명</h3>
      <p>1. '테스트용'이라는 용어는 주로 소프트웨어 개발 및 품질 보증 과정에서 사용됩니다. 이는 특정 기능이나 시스템이 제대로 작동하는지 확인하기 위한 테스트를 수행하는 것을 의미합니다. 개발자와 QA 엔지니어는 다양한 테스트를 통해 코드의 안정성, 성능, 보안성을 검토하며, 사용자 경험을 개선하기 위해 피드백을 받습니다. 이 과정에서 '테스트용' 데이터나 환경이 필요하며, 실제 사용자 정보와는 분리되어 있어야 합니다. 이러한 테스트는 최종 제품이 사용자의 기대에 부응할 수 있도록 보장하는 중요한 단계입니다.</p>
    </div>
  )
}

function DownloadPage(){
  return(
    <div>
      <img src = {logo} style = {{top: "50px"}}></img>
      <h1>게임설명</h1>
      <button>다운로드</button>
    </div>
  )
}
function AnimatedRoutes() {
  const location = useLocation();
  return (
    <>
      <Header/>
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
