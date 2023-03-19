
const Price = ({ item, priceClassName, discountClassName, vertical }) => {
    return (
        <>
            {
                vertical ?
                    <div className="d-flex mb-2 fw-bold">
                        {
                            item.discountPrice ?
                                <div className="d-flex border-gradient justify-content-center align-items-center"
                                    style={{aspectRatio: 1 / 1 }}>
                                    <span className={"text-gradient " + discountClassName}
                                    >
                                        {((item.discountPrice / item.price * 100) - 100).toFixed(0)}%
                                    </span>
                                </div>
                                :
                                <> </>
                        }
                        <div className="d-flex flex-column align-items-center justify-content-center ms-3">
                            {
                                item.discountPrice ?
                                    <span className="position-absolute text-decoration-line-through text-warm fs-6"
                                        style={{ marginTop: '-50px', marginLeft: '-50px' }}>{item.price}$</span>
                                    :
                                    <>
                                    </>
                            }
                            <span className={"p-1 fw-bold " + priceClassName}>{(item.discountPrice ?? item.price)}<sup>$</sup></span>
                        </div>
                    </div>
                    :
                    <div className="d-flex flex-row flex-wrap align-items-center gap-2 justify-content-center">
                        {
                            item.discountPrice ?
                                <>
                                    <span className={"bg-discount p-2 text-white " + discountClassName}>
                                        {((item.discountPrice / item.price * 100) - 100).toFixed(0)}%
                                    </span>
                                </>
                                :
                                <> </>
                        }
                        <span className={"bg-price p-2 text-white " + priceClassName}>{(item.discountPrice ?? item.price).toFixed(1)}<sup>$</sup></span>
                    </div>
            }
        </>
    )
}
export default Price;