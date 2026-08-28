import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { GetGamesByFilters } from "../../../utils/ApiRequests";
import { FilterSearch } from "./parts/filter/FilterSearch";
import GameList from "./parts/GameList";
import './GameSearch.css';

const GameSearch = () => {
    const { t } = useTranslation();
    const [searchParams, setSearchParams] = useSearchParams();
    const [searchFilters, setsearchFilters] = useState({});

    const [games, setGames] = useState([]);
    const [isMax, setIsMax] = useState();
    const [page, setPage] = useState(1);
    const [count, setCount] = useState(0);

    useEffect(() => {
        if (searchParams.get('search')) {
            setGames([]);
            setPage(0);
            setsearchFilters(prevState => ({
                ...prevState,
                searchQuery: searchParams.get('search'),
            }));
            searchParams.delete('id')
            setSearchParams(undefined)
            return
        }
    }, [searchParams]);

    const findUnique = (value) =>{
        if(games.some(game => game.id === value.id)){
            return false;
        }
        return true;
    }

    useEffect(() => {
        if (page === 0) {
            setPage(prevState => prevState + 1)
            return
        }
        GetGamesByFilters(searchFilters, page)
            .then(result => {
                if (result.page !== 1) {
                    setGames(prevState => [...prevState, ...result.games.filter(findUnique)]);
                }else{
                    setGames(result.games)
                }
                setIsMax(result.isMax);
                setCount(result.totalCount);
            });
    }, [searchFilters, page])

    return (
        <div className="h-100 m-4">
            <div id="search-bar-game-search-page" className="search-header">
                <div className="d-flex flex-row align-items-center gap-3">
                    <h3 className="fw-bold m-0">
                        {t('search.found')} {count !== 0 ? t('catalog.gamesCount', { count }) : t('search.noGames')}
                    </h3>
                </div>
                <div className="search-filters mt-3">
                    <FilterSearch searchFilters={searchFilters}
                        setSearchFilter={setsearchFilters}
                        searchQuery={searchFilters ? searchFilters.searchQuery : ''}
                        setPage={setPage}
                    />
                </div>
            </div>
            <div className="d-flex mt-5 flex-row gap-4">
                <div className="flex-fill h-100">
                    <div className="col">
                        <GameList games={games} isMax={isMax} setPage={setPage} />
                    </div>
                </div>
            </div>
        </div>
    )
}

export default GameSearch;