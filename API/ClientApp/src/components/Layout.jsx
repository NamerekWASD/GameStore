import React, { useState } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import Footer from './Footer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUpLong } from '@fortawesome/free-solid-svg-icons';
import { ToastContainer } from 'react-toastify';
import "react-toastify/dist/ReactToastify.css";
import { useScrollPosition } from '@n8tb1t/use-scroll-position';
import $ from 'jquery'

export function scrollToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

const Layout = ({ children, isAuthenticated, refreshAuth }) => {
    const [toTopButtonStyle, setToTopButtonStyle] = useState({
        transition: 'all 200ms ease-in'
    })
    useScrollPosition(({ prevPos, currPos }) => {
        const appearBoundary = -500;
        const shouldAppear = currPos.y < appearBoundary;
        const shouldBeStyle = {
            visibility: shouldAppear ? 'visible' : 'hidden',
            transition: `all 200ms ${shouldAppear ? 'ease-in' : 'ease-out'}`,
            bottom: shouldAppear ? '20px' : '-100px'
        }
        if (JSON.stringify(shouldBeStyle) === JSON.stringify(toTopButtonStyle)) return

        setToTopButtonStyle(shouldBeStyle)
    }, [toTopButtonStyle], false, false, 300);

    return (
        <div id='main'>
            <NavMenu isAuthenticated={isAuthenticated} refreshAuth={refreshAuth} />
            <Container id='content' className='w-100'
                style={{ minHeight: '100vh', margin: 0, padding: 0, maxWidth: '100%', paddingTop: $('#myHeader').outerHeight() + 'px' }}>
                <ToastContainer position="bottom-left"
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
            <button id='to-top-btn' style={{ ...toTopButtonStyle }} onClick={() => scrollToTop()}>
                <FontAwesomeIcon icon={faUpLong} style={{ color: 'white' }} />
            </button>
        </div>
    );
}
export default Layout;