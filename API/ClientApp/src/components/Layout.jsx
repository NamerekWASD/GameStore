import React, { useEffect } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import Footer from './Footer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUpLong } from '@fortawesome/free-solid-svg-icons';
import $ from 'jquery'
import { ToastContainer } from 'react-toastify';
import "react-toastify/dist/ReactToastify.css";

const Layout = ({ children, isAuthenticated, refreshAuth }) => {
  var header, content, toTopBtn;

  useEffect(() => {
    header = document.getElementById("myHeader");
    content = document.getElementById("content");
    toTopBtn = {
      element: $('#to-top-btn'),
      isGone: true,
      inProgress: false
    };
    window.onscroll = () => scrollFunction();
  }, [header, content, toTopBtn])

  function scrollFunction() {
    toggleToTopBtn();
    toggleHeader();
  }

  function toggleToTopBtn() {
    if (toTopBtn.inProgress) {
      return
    }
    const appearBoundary = 500
    let shouldAppear = document.body.scrollTop > appearBoundary
      || document.documentElement.scrollTop > appearBoundary;

    if (shouldAppear && toTopBtn.isGone) {
      checkButton(false, 20)
    }
    if (!shouldAppear && !toTopBtn.isGone) {
      checkButton(true, -100)
    }
    function checkButton(flag, yValue) {
      toTopBtn.inProgress = true;
      toTopBtn.element.animate({ bottom: yValue }, 'fast', () => {
        toTopBtn.isGone = flag;
        toTopBtn.inProgress = false;
      })
    }
  }

  function toggleHeader() {
    const sticky = header.offsetTop;
    if (window.pageYOffset > sticky) {
      header.classList.add("sticky");
      content.style.paddingTop = header.offsetHeight + 'px';
    } else {
      header.classList.remove("sticky");
      content.style.paddingTop = 0;
    }
  }

  function scrollToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }
  return (
    <div id='main'>
      <NavMenu isAuthenticated={isAuthenticated} refreshAuth={refreshAuth}/>
      <Container id='content' className='w-100' style={{ minHeight: '100vh', margin: 0, padding: 0, maxWidth: '100%' }}>
      <ToastContainer position="top-center"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="dark" />
        {children}
      </Container>
      <Footer />
      <button id='to-top-btn' onClick={() => scrollToTop()}>
        <FontAwesomeIcon icon={faUpLong} style={{ color: 'white' }} />
      </button>
    </div>
  );
}
export default Layout;