export const FilterSearch = ({ searchFilters, setSearchFilter, searchQuery, setPage }) => {
    return (
        <div>
            <input type="text" className="form-control w-100 rounded-0"
                name="search-query" id="search-query" defaultValue={searchQuery}
                onChange={(e) => {
                    const value = e.target.value;
                    if (value.length > 2) {
                        setSearchFilter(previousState => ({
                            ...previousState,
                            searchQuery: value
                        }))
                        setPage(0);
                        return;
                    }

                    if(searchFilters.searchQuery.length !== 0 && value.length <= 2){
                        setSearchFilter(previousState => ({
                            ...previousState,
                            searchQuery: ''
                        }))
                        setPage(0);
                    }
                }}
                placeholder="Пошук..."
            />
        </div>
    )
}