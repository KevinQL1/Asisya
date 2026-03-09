import { useEffect, useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../api/axiosConfig';

const Products = () => {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [search, setSearch] = useState('');
    const [page, setPage] = useState(1);
    const [inputPage, setInputPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(false);
    
    const [editingProduct, setEditingProduct] = useState(null);
    const [isCreating, setIsCreating] = useState(false);
    const [newProduct, setNewProduct] = useState({ productName: '', categoryId: '', unitPrice: 0, unitsInStock: 0 });

    const [bulkCount, setBulkCount] = useState(100);
    const [isBulkLoading, setIsBulkLoading] = useState(false);

    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const fetchCategories = async () => {
        try {
            const response = await api.get('/Category');
            setCategories(response.data || []);
        } catch (error) {
            console.error("Error al cargar categorías", error);
        }
    };

    const fetchProducts = async () => {
        setLoading(true);
        try {
            const response = await api.get(`/Product?page=${page}&pageSize=10&search=${search}`);
            setProducts(response.data.data || []);
            setTotalPages(response.data.totalPages > 0 ? response.data.totalPages : 1);
            setInputPage(page);
        } catch (error) {
            console.error("Error al cargar productos", error);
            setProducts([]);
        } finally {
            setLoading(false);
        }
    };

    const handleBulkInsert = async () => {
        if (bulkCount < 100 || bulkCount > 100000) {
            alert("La cantidad debe estar entre 100 y 100,000");
            return;
        }
        setIsBulkLoading(true);
        try {
            await api.post(`/Product/bulk?count=${bulkCount}`);
            alert(`${bulkCount} productos generados exitosamente.`);
            setPage(1);
            fetchProducts();
        } catch (error) {
            alert("Error en la carga masiva.");
        } finally {
            setIsBulkLoading(false);
        }
    };

    const handleJumpPage = (e) => {
        e.preventDefault();
        const p = parseInt(inputPage);
        if (!isNaN(p) && p >= 1 && p <= totalPages) {
            setPage(p);
        } else {
            alert(`Por favor ingresa una página válida entre 1 y ${totalPages}`);
            setInputPage(page);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este producto?")) {
            try {
                await api.delete(`/Product/${id}`);
                alert("Producto eliminado");
                fetchProducts();
            } catch (error) {
                alert("Error al eliminar. Verifique permisos.");
            }
        }
    };

    const handleUpdate = async (e) => {
        e.preventDefault();
        try {
            const selectedCat = categories.find(c => c.categoryId === parseInt(editingProduct.categoryId));
            const updatedProduct = {
                ...editingProduct,
                categoryPicture: selectedCat?.picture || ''
            };
            
            await api.put(`/Product/${updatedProduct.productId}`, updatedProduct);
            alert("Producto actualizado con éxito");
            setEditingProduct(null);
            fetchProducts();
        } catch (error) {
            alert("Error al actualizar");
        }
    };

    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            await api.post('/Product', newProduct);
            alert("Producto creado con éxito");
            setIsCreating(false);
            setNewProduct({ productName: '', categoryId: '', unitPrice: 0, unitsInStock: 0 });
            fetchProducts();
        } catch (error) {
            alert("Error al crear el producto");
        }
    };

    useEffect(() => {
        fetchProducts();
        fetchCategories();
    }, [page, search]);

    return (
        <div style={{ fontFamily: 'sans-serif', padding: '110px 30px 30px 30px' }}>
            <h1 style={{ textAlign: 'center' }}>Listado de Productos</h1>

            {token && (
                <div style={{ marginBottom: '20px', padding: '20px', backgroundColor: '#f4f4f4', borderRadius: '10px', display: 'flex', gap: '15px', justifyContent: 'center', alignItems: 'center', border: '1px solid #ddd' }}>
                    <strong style={{ color: '#333' }}>Generación Masiva:</strong>
                    <input 
                        type="number" 
                        min="100" 
                        max="100000" 
                        value={bulkCount} 
                        onChange={(e) => setBulkCount(parseInt(e.target.value))} 
                        style={{ width: '120px', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
                    />
                    <button 
                        onClick={handleBulkInsert} 
                        disabled={isBulkLoading || bulkCount < 100 || bulkCount > 100000}
                        style={{ backgroundColor: '#17a2b8', color: 'white', border: 'none', padding: '10px 20px', borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold', opacity: (isBulkLoading || bulkCount < 100 || bulkCount > 100000) ? 0.6 : 1 }}
                    >
                        {isBulkLoading ? "Generando..." : "Ejecutar Carga Masiva"}
                    </button>
                </div>
            )}

            <div style={{ marginBottom: '30px', textAlign: 'center', display: 'flex', justifyContent: 'center', gap: '15px' }}>
                <input
                    type="text"
                    placeholder="Buscar por nombre..."
                    value={search}
                    onChange={(e) => { setSearch(e.target.value); setPage(1); }}
                    style={{ padding: '12px', width: '350px', borderRadius: '6px', border: '1px solid #ccc' }}
                />
                {token && (
                    <button onClick={() => setIsCreating(true)} style={{ backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer', padding: '0 20px', fontWeight: 'bold' }}>+ Nuevo Producto</button>
                )}
            </div>

            {loading ? <p style={{ textAlign: 'center' }}>Cargando inventario masivo...</p> : (
                <>
                    <table style={{ width: '100%', borderCollapse: 'collapse', boxShadow: '0 2px 8px rgba(0,0,0,0.1)' }}>
                        <thead style={{ backgroundColor: '#f8f9fa' }}>
                            <tr>
                                <th style={{ padding: '15px', borderBottom: '2px solid #dee2e6' }}>Foto</th>
                                <th style={{ padding: '15px', borderBottom: '2px solid #dee2e6' }}>Producto</th>
                                <th style={{ padding: '15px', borderBottom: '2px solid #dee2e6' }}>Precio</th>
                                <th style={{ padding: '15px', borderBottom: '2px solid #dee2e6' }}>Stock</th>
                                {token && <th style={{ padding: '15px', borderBottom: '2px solid #dee2e6' }}>Acciones</th>}
                            </tr>
                        </thead>
                        <tbody>
                            {products.map(p => (
                                <tr key={p.productId} style={{ textAlign: 'center' }}>
                                    <td style={{ padding: '12px', borderBottom: '1px solid #eee' }}>
                                        {p.categoryPicture ? (
                                            <img src={p.categoryPicture} alt="cat" style={{ width: '45px', height: '45px', objectFit: 'cover', borderRadius: '5px' }} />
                                        ) : ( <span style={{ color: '#ccc' }}>N/A</span> )}
                                    </td>
                                    <td style={{ padding: '12px', borderBottom: '1px solid #eee' }}>{p.productName}</td>
                                    <td style={{ padding: '12px', borderBottom: '1px solid #eee', fontWeight: 'bold' }}>${p.unitPrice}</td>
                                    <td style={{ padding: '12px', borderBottom: '1px solid #eee' }}>{p.unitsInStock}</td>
                                    {token && (
                                        <td style={{ padding: '12px', borderBottom: '1px solid #eee' }}>
                                            <button onClick={() => setEditingProduct(p)} style={{ marginRight: '8px', cursor: 'pointer' }}>Editar</button>
                                            <button onClick={() => handleDelete(p.productId)} style={{ color: '#dc3545', cursor: 'pointer' }}>Borrar</button>
                                        </td>
                                    )}
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    <div style={{ marginTop: '30px', display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '15px' }}>
                        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '8px' }}>
                            <button disabled={page === 1} onClick={() => setPage(1)} style={{ padding: '8px 12px' }}>{"<<"} Primera</button>
                            <button disabled={page === 1} onClick={() => setPage(p => p - 1)} style={{ padding: '8px 12px' }}>Anterior</button>
                            <span style={{ margin: '0 15px', fontSize: '1.1rem' }}>Página <strong>{page}</strong> de {totalPages}</span>
                            <button disabled={page === totalPages} onClick={() => setPage(p => p + 1)} style={{ padding: '8px 12px' }}>Siguiente</button>
                            <button disabled={page === totalPages} onClick={() => setPage(totalPages)} style={{ padding: '8px 12px' }}>Última {">>"}</button>
                        </div>
                        <form onSubmit={handleJumpPage} style={{ display: 'flex', gap: '10px', alignItems: 'center' }}>
                            <label style={{ fontSize: '0.9rem' }}>Ir a la página:</label>
                            <input 
                                type="number" 
                                min="1" 
                                max={totalPages} 
                                value={inputPage} 
                                onChange={(e) => setInputPage(e.target.value)}
                                style={{ width: '80px', padding: '5px', textAlign: 'center', borderRadius: '4px', border: '1px solid #ccc' }}
                            />
                            <button type="submit" style={{ padding: '5px 10px', cursor: 'pointer' }}>Saltar</button>
                        </form>
                    </div>
                </>
            )}

            {(editingProduct || isCreating) && (
                <div style={{ position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 4000 }}>
                    <form 
                        onSubmit={editingProduct ? handleUpdate : handleCreate} 
                        style={{ backgroundColor: 'white', padding: '35px', borderRadius: '12px', display: 'flex', flexDirection: 'column', gap: '12px', width: '450px', boxShadow: '0 10px 25px rgba(0,0,0,0.2)' }}
                    >
                        <h2 style={{ marginTop: 0 }}>{editingProduct ? 'Editar Producto' : 'Nuevo Registro'}</h2>
                        <label>Nombre del Producto:</label>
                        <input required value={editingProduct?.productName || newProduct.productName} onChange={e => editingProduct ? setEditingProduct({...editingProduct, productName: e.target.value}) : setNewProduct({...newProduct, productName: e.target.value})} style={{ padding: '10px' }} />
                        <label>Categoría Asociada:</label>
                        <select 
                            required 
                            value={editingProduct?.categoryId || newProduct.categoryId} 
                            onChange={e => {
                                const val = parseInt(e.target.value);
                                editingProduct ? setEditingProduct({...editingProduct, categoryId: val}) : setNewProduct({...newProduct, categoryId: val});
                            }}
                            style={{ padding: '10px' }}
                        >
                            <option value="">Seleccione una categoría</option>
                            {categories.map(cat => (
                                <option key={cat.categoryId} value={cat.categoryId}>{cat.categoryName}</option>
                            ))}
                        </select>
                        <div style={{ display: 'flex', gap: '15px' }}>
                            <div style={{ flex: 1 }}>
                                <label>Precio ($):</label>
                                <input required type="number" step="0.01" value={editingProduct?.unitPrice || newProduct.unitPrice} onChange={e => {
                                    const val = parseFloat(e.target.value);
                                    editingProduct ? setEditingProduct({...editingProduct, unitPrice: val}) : setNewProduct({...newProduct, unitPrice: val});
                                }} style={{ padding: '10px', width: '100%' }} />
                            </div>
                            <div style={{ flex: 1 }}>
                                <label>Stock Inicial:</label>
                                <input required type="number" value={editingProduct?.unitsInStock || newProduct.unitsInStock} onChange={e => {
                                    const val = parseInt(e.target.value);
                                    editingProduct ? setEditingProduct({...editingProduct, unitsInStock: val}) : setNewProduct({...newProduct, unitsInStock: val});
                                }} style={{ padding: '10px', width: '100%' }} />
                            </div>
                        </div>
                        <div style={{ marginTop: '20px', display: 'flex', gap: '12px' }}>
                            <button type="submit" style={{ backgroundColor: '#007bff', color: 'white', padding: '12px', flex: 2, border: 'none', borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold' }}>
                                {editingProduct ? 'Guardar Cambios' : 'Registrar Producto'}
                            </button>
                            <button type="button" onClick={() => {setEditingProduct(null); setIsCreating(false);}} style={{ backgroundColor: '#eee', padding: '12px', flex: 1, border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Cancelar</button>
                        </div>
                    </form>
                </div>
            )}
        </div>
    );
};

export default Products;