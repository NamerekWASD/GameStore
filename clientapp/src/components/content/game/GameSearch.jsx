import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { GetGamesByFilters } from "../../../utils/ApiRequests";
import { processHeader } from "./GameCatalog";
import { FilterSearch } from "./parts/filter/FilterSearch";
import GameList from "./parts/GameList";

const GameSearch = () => {
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
            <div>
                <div className="d-flex flex-row">
                    <h3 className="fw-bold">Знайдено {count !== 0 ? processHeader(count) : 'жодної гри'}</h3>
                </div>
                <FilterSearch searchFilters={searchFilters}
                setSearchFilter={setsearchFilters}
                searchQuery={searchFilters ? searchFilters.searchQuery : ''}
                setPage={setPage}/>
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