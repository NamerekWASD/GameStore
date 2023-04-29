import React, { useEffect, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Select from "react-select";
import { GetGamesByFilters } from "../../../utils/ApiRequests";
import { orderOptions } from "../../../utils/Constants";
import './game.css'
import FilterTable from "./parts/filter/FilterTable";
import GameList from "./parts/GameList";

export const processHeader = (count) => {
    switch (true) {
        case (count === 1):
            return count + ' гра';
        case (count >= 2 && count < 5):
            return count + ' гри';
        case (count >= 5):
            return count + ' ігор';
        default:
            return count + ' ігор';
    }
}

const GameCatalog = () => {
    const [searchParams] = useSearchParams();
    const genreRef = useRef(null);
    const [searchFilters, setSearchFilters] = useState({ genreIds: [], regionIds: [], platformIds: [] });

    const [games, setGames] = useState([]);
    const [isMax, setIsMax] = useState(false);
    const [page, setPage] = useState(1);
    const [count, setCount] = useState(0);

    useEffect(() => {
        genreRef.current.textContent = searchParams.get('genre') ?? 'Каталог';
        if (searchParams.get('id')) {
            setSearchFilters(prevState => ({
                ...prevState,
                genreIds: [+searchParams.get('id')]
            }))
        }else{
            setSearchFilters(prevState => ({
                ...prevState,
                genreIds: []
            }))
        }
    }, [searchParams])

    useEffect(() => {
        if (page === 0) {
            setPage(prevState => prevState + 1)
            return
        }
        GetGamesByFilters(searchFilters, page)
            .then(result => {
                if (result.page !== 1) {
                    setGames(prevState => [...prevState, ...result.games]);
                } else {
                    setGames(result.games)
                }
                setIsMax(result.isMax);
                setCount(result.totalCount);
            });
    }, [searchParams, page, searchFilters]);

    return (
        <div className="h-100 m-4">
            <div>
                <h1 ref={genreRef} className="fw-bolder">Каталог</h1>
                <div className="d-flex flex-row">
                    <div className="fw-bold">Знайдено {count !== 0 ? processHeader(count) : 'жодної гри'}</div>
                    <div className="ms-5 w-25">
                        <Select className="" id="order-list"
                            options={orderOptions}
                            defaultValue={orderOptions[0]}
                            onChange={(e) => {
                                setSearchFilters(prevState => ({
                                    ...prevState,
                                    orderBy: parseInt(e.value),
                                }))
                                setPage(0);
                            }}
                            placeholder="Упорядкувати"
                        />
                    </div>
                </div>
            </div>
            <div className="d-flex mt-5 flex-row gap-4">
                <div className="flex-fill h-100">
                    <div className="col">
                        <GameList games={games} isMax={isMax} setPage={setPage} />
                    </div>
                </div>
                <FilterTable setGames={setGames}
                    setSearchFilters={setSearchFilters}
                    setPage={setPage}
                    searchFilters={searchFilters}
                    showGenre={!searchParams.get('id')}
                />
            </div>
        </div>
    );
}
export default GameCatalog;