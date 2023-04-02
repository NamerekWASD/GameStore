
export const FilterSearch = ({ setSearchFilter, searchQuery, setPage }) => {

    return (
        <div>
            <input type="text" className="form-control w-100 rounded-0"
                name="search-query" id="search-query" defaultValue={searchQuery}
                onChange={(e) => {
                    setSearchFilter(previousState => ({
                        ...previousState,
                        searchQuery: e.target.value
                    }))
                    setPage(0);
                }} 
                placeholder="Пошук..."
                />
        </div>
    )
}