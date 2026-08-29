import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next";
import { navigateToDetails } from "../../../../utils/Navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import Price from "./Price";
import { POSTER } from "../../../../utils/Constants";
import { useMediaQuery } from "../../../../utils/useMediaQuery";
import { VideoContainer } from "./VideoContainer";

const MOBILE_QUERY = '(max-width: 800px)';

function chunk(array, size) {
    const result = [];
    for (let i = 0; i < array.length; i += size) {
        result.push(array.slice(i, i + size));
    }
    return result;
}

const Carousel = ({ games }) => {
    const { t } = useTranslation();
    const container = useRef(null);
    const timeoutRef = useRef(null);
    const touchStartXRef = useRef(null);
    const isMobileLayout = useMediaQuery(MOBILE_QUERY);
    const navigate = useNavigate();
    const [sliderIndex, setSliderIndex] = useState(0);
    const slides = useMemo(() => chunk(games, isMobileLayout ? 1 : 3), [games, isMobileLayout]);

    const clearTimeOut = () => {
        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }
    }

    const goToNext = () => {
        setSliderIndex(prevIndex => slides.length ? (prevIndex + 1) % slides.length : 0);
    }

    const goToPrev = () => {
        setSliderIndex(prevIndex => slides.length ? (prevIndex - 1 + slides.length) % slides.length : 0);
    }

    const startCarousel = () => {
        clearTimeOut();
        timeoutRef.current = setTimeout(goToNext, 5000);
    }

    useEffect(() => {
        setSliderIndex(0);
    }, [slides.length])

    useEffect(() => {
        startCarousel()
        return () => clearTimeOut();
    }, [sliderIndex])

    useEffect(() => {
        setSliderIndex(0);
    }, [isMobileLayout])

    useEffect(() => {
        window.addEventListener('blur', clearTimeOut);
        window.addEventListener('focus', startCarousel);
        return () => {
            window.removeEventListener('blur', clearTimeOut);
            window.removeEventListener('focus', startCarousel);
        }
    }, [])

    const handleTouchStart = (e) => {
        touchStartXRef.current = e.touches[0].clientX;
    }

    const handleTouchEnd = (e) => {
        if (touchStartXRef.current === null) return;
        const deltaX = e.changedTouches[0].clientX - touchStartXRef.current;
        touchStartXRef.current = null;
        if (Math.abs(deltaX) < 50) return;
        deltaX < 0 ? goToNext() : goToPrev();
    }

    const handleKeyDown = (e) => {
        if (e.key === 'ArrowLeft') {
            e.preventDefault();
            goToPrev();
        } else if (e.key === 'ArrowRight') {
            e.preventDefault();
            goToNext();
        }
    }

    const handleBlur = (e) => {
        if (!e.currentTarget.contains(e.relatedTarget)) {
            startCarousel();
        }
    }

    function renderCard(game, extraClassName = '') {
        if (!game) return null;
        const poster = game.images?.find(value => value.type?.name === POSTER);
        if (!poster) return null;
        return (
            <div className={`pointer carousel-card ${extraClassName}`.trim()}
                onClick={() => navigateToDetails(game, navigate)}>
                <img className="carousel-card-image" src={poster.path} alt={game.title} />
                <div className="carousel-card-gradient" />
                <div className="carousel-card-info">
                    <h5 className="carousel-card-title">{game.title}</h5>
                    {
                        game.discountPrice ?
                            <Price item={game} priceClassName="p-1" discountClassName="p-1" />
                            : null
                    }
                </div>
            </div>
        )
    }

    return (
        <>
            <div className="video-container">
                {
                    isMobileLayout ? '' : <VideoContainer />
                }
                <div
                    ref={container}
                    className="d-flex flex-column justify-content-center align-items-center content bg-dark px-3 py-3"
                    style={{ maxWidth: '100%', height: '80vh' }}
                    role="region"
                    aria-roledescription="carousel"
                    aria-label={t('carousel.regionLabel')}
                    tabIndex={0}
                    onMouseEnter={clearTimeOut}
                    onMouseLeave={startCarousel}
                    onFocus={clearTimeOut}
                    onBlur={handleBlur}
                    onKeyDown={handleKeyDown}
                >
                    <div
                        className="carousel-viewport-wrapper"
                        style={isMobileLayout ? { width: '100%', height: '100%', minHeight: 0 } : { width: 'min(750px, 100%)', height: '100%', minHeight: 0 }}
                        onTouchStart={handleTouchStart}
                        onTouchEnd={handleTouchEnd}
                    >
                        {
                            slides.length > 1 &&
                            <button type="button" className="carousel-arrow carousel-arrow-prev"
                                aria-label={t('carousel.previousSlide')} onClick={goToPrev}>
                                <FontAwesomeIcon icon={faChevronLeft} />
                            </button>
                        }
                        <div className="overflow-hidden content w-100 h-100">
                            <div className="carousel-track d-flex flex-row" style={{ transform: `translateX(-${sliderIndex * 100}%)` }}>
                                {
                                    slides.map((slide, index) => {
                                        return (
                                            <div key={index} className={isMobileLayout ? "d-flex flex-row my-carousel-item carousel-slide" : "d-flex flex-row my-carousel-item carousel-slide carousel-hero-row"}>
                                                {
                                                    isMobileLayout ?
                                                        renderCard(slide[0], 'carousel-mobile-main')
                                                        :
                                                        <>
                                                            {renderCard(slide[0], 'carousel-hero-main')}
                                                            <div className="carousel-hero-side">
                                                                {renderCard(slide[1])}
                                                                {renderCard(slide[2])}
                                                            </div>
                                                        </>
                                                }
                                            </div>
                                        )
                                    })
                                }
                            </div>
                        </div>
                        {
                            slides.length > 1 &&
                            <button type="button" className="carousel-arrow carousel-arrow-next"
                                aria-label={t('carousel.nextSlide')} onClick={goToNext}>
                                <FontAwesomeIcon icon={faChevronRight} />
                            </button>
                        }
                    </div>

                    <div className="carousel-bullets">
                        {
                            slides.map((_, index) => {
                                return (
                                    <button
                                        key={index}
                                        type="button"
                                        className={index === sliderIndex ? 'carousel-bullet active' : 'carousel-bullet'}
                                        aria-label={t('carousel.goToSlide', { number: index + 1 })}
                                        aria-current={index === sliderIndex ? 'true' : undefined}
                                        onClick={() => setSliderIndex(index)} />
                                )
                            })
                        }
                    </div>
                </div>
            </div>
        </>
    )
}

export default Carousel;
