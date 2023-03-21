import React, { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { loadFilterData, loadGames, loadGamesByFilters, loadGamesByGenre, loadGenres } from "../../../utils/ApiRequests";
import './game.css'
import FilterTable from "./parts/FilterTable";
import GameList from "./parts/GameList";

const GameCatalog = () => {
    const [searchParams] = useSearchParams();
    const [games, setGames] = useState([]);
    const genreRef = useRef(null);

    const order = {
        names: { name: 'title', type: 'desc' },
        newer: { name: 'released', type: 'asc' },
        older: { name: 'released', type: 'desc' },
        expensivest: { name: 'price', type: 'asc' },
        cheaper: { name: 'price', type: 'desc' },
        popularity: { name: 'sold', type: 'asc' }
    }
    const [sortType, setSortType] = useState(order.names);

    useEffect(() => {
        genreRef.current.textContent = searchParams.get('genre') ?? 'Каталог';
        if (!searchParams.get('id')) {
            loadGames(true)
            .then(result => {
                setGames(result);
            });
        } else {
            loadGamesByFilters({ genreIds: [searchParams.get('id')] })
            .then(result => {
                setGames(result);
            });
        }

    }, [searchParams]);

    function renderGames() {
        const copy = games.sort(function (a, b) {
            var propertyA, propertyB;
            if (sortType.name === order.newer.name) {
                propertyA = new Date(Date.parse(a[sortType.name]));
                propertyB = new Date(Date.parse(b[sortType.name]));
            } else {
                propertyA = a[sortType.name];
                propertyB = b[sortType.name];
            }
            if (propertyA < propertyB) return -1;
            if (propertyA > propertyB) return 1;
            return 0
        });
        if (sortType.type === 'asc') {
            copy.reverse();
        }
        return (
            <>
                {
                    (copy.length !== 0) ?
                        <GameList games={copy} />
                        : <h3>За вашим запитом не знайдено жодної гри</h3>
                }
            </>
        )
    }
    return (
        <div className="container h-100 mt-4">
            <div>
                <h1 ref={genreRef} className="fw-bolder"></h1>
                <div className="d-flex flex-row">
                    <div className="fw-bold">Знайдено {games.length} {games.length.toString().endsWith('1') ? 'гра' : 'гри'}</div>
                    <div className="ms-5">
                        Упорядкувати
                        <select className="ms-2 no-outline overflow-scroll" id="order-list"
                            style={{ width: '200px' }} onChange={(e) => {
                                setSortType(order[e.target.value])
                            }}>
                            <option value={"names"}>За назвою</option>
                            <option value={"newer"}>За датою, від нових</option>
                            <option value={"older"}>За датою, від старих</option>
                            <option value={"expensivest"}>За ціною, від дорогих</option>
                            <option value={"cheaper"}>За ціною, від дешевих</option>
                            <option value={"popularity"}>За популярністю</option>
                        </select>
                    </div>
                </div>
            </div>
            <div className="d-flex mt-5 flex-row gap-4">
                <div className="flex-fill h-100">
                    <div className="col">
                        {renderGames()}
                    </div>
                </div>
                <FilterTable setGames={setGames} searchFilter={searchParams.get('id') ? {genreIds: [searchParams.get('id')]} : undefined} />
            </div>
        </div>
    );
}
export default GameCatalog;
