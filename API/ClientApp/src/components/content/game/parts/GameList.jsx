import React, { useRef } from "react";
import { useNavigate } from "react-router-dom";
import LoadingCircle from "../../../../utils/LoadingCircle";
import { navigateToDetails } from '../../../../utils/Navigation';
import './../game.css';
import Price from "./Price";
import { useScrollPosition } from '@n8tb1t/use-scroll-position'
import { PORTRAIT } from "../../../../utils/Constants";
import { Card } from 'react-bootstrap';

const GameList = ({ games, isMax, setPage, isVertical, noNeedPagination, cardClassName, isColumn }) => {
    const navigate = useNavigate();
    const listContainer = useRef(null);
    useScrollPosition(({ prevPos, currPos }) => {
        if(noNeedPagination) return;
        const rect = listContainer.current.getBoundingClientRect();
        const bottomPosition = rect.height + rect.y + currPos.y - window.innerHeight;
        if (bottomPosition < 0 && !isMax) {
            setPage(prevState => prevState + 1);
        }

    }, [isMax], false, false, 300)

    function renderHorizontal() {
        return (
            <div ref={listContainer} className="text-white">
                {
                    (games.length !== 0) ?
                        games.map(game => {
                            return (
                                <div key={game.id} className={"card rounded-0 w-100 bg-light-gray " + cardClassName} style={{ cursor: 'pointer', height: '180px' }}
                                    onClick={() => navigateToDetails(game, navigate)} >
                                    <div className="no-gutters d-flex flex-row align-items-center gap-5">
                                        <div style={{ minWidth: "120px", width: "120px", maxWidth: "120px", height: '150px' }} className="overflow-hidden m-3">
                                            <img src={game.image ? game.image.path : game.images.find(item => item.type.name === PORTRAIT).path} alt={game.title} className="width-inherit" />
                                        </div>
                                        <div className="flex-fill">
                                            <h6 className="card-title">{game.title}</h6>
                                            <h6>{game.genres.join(' ')}</h6>
                                        </div>
                                        <div className="p-3">
                                            <Price item={game} />
                                        </div>
                                    </div>
                                </div>
                            )
                        }) : <></>
                }
            </div>
        )
    }
    function renderVertical() {
        return (
            <div ref={listContainer} className="text-white" style={{ display: "flex", flexDirection: isColumn ? 'column' : 'row' }}>
                {
                    (games.length !== 0) ?
                        games.map(game => {
                            return (
                                <Card key={game.id} className={"m-2 bg-light-gray rounded-0 overflow-hidden pointer " + cardClassName} onClick={() => navigateToDetails(game, navigate)} >
                                    <Card.Img variant="top" 
                                    src={game.image ? game.image.path : game.images.find(item => item.type.name === PORTRAIT).path} 
                                    alt={game.title} style={{width: '270px', height:'320px'}}
                                    className="rounded-0"/>
                                    <Card.Body>
                                        <Card.Title className="fs-6">{game.title}</Card.Title>
                                        <Card.Text>{game.genres.join(' ')}</Card.Text>
                                        <Price item={game} discountClassName={"bg-discount-red"} />
                                    </Card.Body>
                                </Card>
                            )
                        }) : <></>
                }
            </div>
        )
    }
    return (
        <div className="g-4 justify-content-center flex-wrap">
            {
                isVertical ?
                    renderVertical()
                    :
                    renderHorizontal()
            }
            {
                !isMax ?
                    <LoadingCircle />
                    :
                    ''
            }
        </div>
    )
}
export default GameList;