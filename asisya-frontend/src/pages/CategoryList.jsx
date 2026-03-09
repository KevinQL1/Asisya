import { useState, useEffect } from 'react';
import api from '../api/axiosConfig';

const CategoryList = () => {
    const [categories, setCategories] = useState([]);
    const [newCategory, setNewCategory] = useState({
        categoryName: '',
        description: '',
        picture: ''
    });
    const [loading, setLoading] = useState(false);

    const fetchCategories = async () => {
        try {
            const response = await api.get('/Category');
            setCategories(response.data);
        } catch (error) {
            console.error("Error al cargar categorías:", error);
        }
    };

    useEffect(() => {
        fetchCategories();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            await api.post('/Category', newCategory);
            setNewCategory({ categoryName: '', description: '', picture: '' });
            await fetchCategories();
            alert("Categoría creada exitosamente");
        } catch (error) {
            alert("Error al crear la categoría.");
        } finally {
            setLoading(false);
        }
    };

    const inputStyle = {
        width: '100%',
        padding: '15px',
        borderRadius: '8px',
        border: '1px solid #ccc',
        backgroundColor: '#f8faff',
        fontSize: '16px',
        outline: 'none'
    };

    const groupStyle = {
        marginBottom: '25px'
    };

    const labelStyle = {
        display: 'block',
        fontWeight: 'bold',
        marginBottom: '10px',
        fontSize: '15px',
        color: '#333'
    };

    return (
        <div style={{ padding: '120px 20px 60px', maxWidth: '700px', margin: '0 auto', fontFamily: 'sans-serif' }}>

            <form onSubmit={handleSubmit} style={{
                backgroundColor: 'white',
                padding: '40px',
                borderRadius: '15px',
                boxShadow: '0 4px 25px rgba(0,0,0,0.1)',
                marginBottom: '50px'
            }}>
                <h2 style={{ textAlign: 'center', marginBottom: '40px', color: '#1a1a1a', fontSize: '28px' }}>
                    Registro de Nueva Categoría
                </h2>

                <div style={groupStyle}>
                    <label style={labelStyle}>Nombre de Categoría *</label>
                    <input
                        required
                        type="text"
                        value={newCategory.categoryName}
                        onChange={(e) => setNewCategory({ ...newCategory, categoryName: e.target.value })}
                        style={inputStyle}
                        placeholder="Ej: Cloud"
                    />
                </div>

                <div style={groupStyle}>
                    <label style={labelStyle}>URL de la Imagen *</label>
                    <input
                        required
                        type="url"
                        value={newCategory.picture}
                        onChange={(e) => setNewCategory({ ...newCategory, picture: e.target.value })}
                        style={inputStyle}
                        placeholder="https://ejemplo.com/imagen.jpg"
                    />
                </div>

                <div style={groupStyle}>
                    <label style={labelStyle}>Descripción</label>
                    <textarea
                        value={newCategory.description}
                        onChange={(e) => setNewCategory({ ...newCategory, description: e.target.value })}
                        style={{ ...inputStyle, height: '120px', resize: 'none' }}
                        placeholder="Escribe los detalles aquí..."
                    />
                </div>

                <button
                    type="submit"
                    disabled={loading}
                    style={{
                        width: '100%',
                        padding: '18px',
                        backgroundColor: '#28a745',
                        color: 'white',
                        border: 'none',
                        borderRadius: '10px',
                        cursor: 'pointer',
                        fontWeight: 'bold',
                        fontSize: '18px',
                        marginTop: '10px'
                    }}
                >
                    {loading ? 'Guardando...' : 'Finalizar Registro de Categoría'}
                </button>
            </form>

            <div style={{ backgroundColor: 'white', borderRadius: '15px', boxShadow: '0 4px 20px rgba(0,0,0,0.08)', overflow: 'hidden' }}>
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ backgroundColor: '#222', color: 'white', textAlign: 'left' }}>
                            <th style={{ padding: '20px' }}>Imagen</th>
                            <th style={{ padding: '20px' }}>Nombre</th>
                            <th style={{ padding: '20px' }}>Descripción</th>
                        </tr>
                    </thead>
                    <tbody>
                        {categories.map((cat) => (
                            <tr key={cat.categoryId} style={{ borderBottom: '1px solid #eee' }}>
                                <td style={{ padding: '15px', textAlign: 'center' }}>
                                    <img
                                        src={cat.picture}
                                        alt=""
                                        style={{ width: '60px', height: '60px', objectFit: 'cover', borderRadius: '8px' }}
                                        onError={(e) => { e.target.src = 'https://via.placeholder.com/60' }}
                                    />
                                </td>
                                <td style={{ padding: '20px', fontWeight: 'bold' }}>{cat.categoryName}</td>
                                <td style={{ padding: '20px', color: '#555' }}>{cat.description}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default CategoryList;