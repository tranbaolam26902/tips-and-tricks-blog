// Libraries
import { useEffect, useState } from 'react';
import { Table } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// App's features
import {
	deleteCategoryById,
	getCategoriesByQueries,
} from '../../../services/categories';

// App's components
import Loading from '../../../components/Loading';
import Pager from '../../../components/blog/Pager';
import CategoryFilterPane from '../../../components/admin/CategoryFilterPane';

export default function Categories() {
	// Component's states
	const [pageNumber, setPageNumber] = useState(1);
	const [keyword, setKeyword] = useState('');
	const [showOnMenu, setShowOnMenu] = useState(false);
	const [isLoading, setIsLoading] = useState(true);
	const [metadata, setMetadata] = useState({});
	const [categories, setCategories] = useState([]);
	const [isChangeStatus, setIsChangeStatus] = useState(false);

	// Component's event handlers
	const handleChangePage = (value) => {
		setPageNumber((current) => current + value);
		window.scroll(0, 0);
	};
	const handleDeleteCategory = async (e, id) => {
		if (window.confirm('Bạn có chắc muốn xóa chủ đề?')) {
			const data = await deleteCategoryById(id);
			if (data.isSuccess) alert(data.result);
			else alert(data.errors[0]);
			setIsChangeStatus(!isChangeStatus);
		}
	};

	useEffect(() => {
		document.title = 'Danh sách chủ đề';
		fetchCategories();

		async function fetchCategories() {
			const queries = new URLSearchParams({
				PageNumber: pageNumber || 1,
				PageSize: 10,
				ShowOnMenu: showOnMenu || false,
			});
			keyword && queries.append('Keyword', keyword);

			const data = await getCategoriesByQueries(queries);
			if (data) {
				setCategories(data.items);
				setMetadata(data.metadata);
			} else {
				setCategories([]);
				setMetadata({});
			}
			setIsLoading(false);
		}
	}, [pageNumber, keyword, showOnMenu, isChangeStatus]);

	return (
		<div className='mb-5'>
			<h1>Danh sách chủ đề</h1>
			<CategoryFilterPane
				setKeyword={setKeyword}
				setShowOnMenu={setShowOnMenu}
			/>
			{isLoading ? (
				<Loading />
			) : (
				<>
					<Table striped responsive bordered>
						<thead>
							<tr>
								<th>Chủ đề</th>
								<th>Hiện trên menu</th>
								<th>Số bài viết</th>
								<th>Xóa</th>
							</tr>
						</thead>
						<tbody>
							{categories.length > 0 ? (
								categories.map((category) => (
									<tr key={category.id}>
										<td>
											<Link
												to={`/admin/categories/edit/${category.id}`}
												className='text-bold'
											>
												{category.name}
											</Link>
										</td>
										<td>
											{category.showOnMenu
												? 'Có'
												: 'Không'}
										</td>
										<td>{category.postCount}</td>
										<td>
											<button
												onClick={(e) =>
													handleDeleteCategory(
														e,
														category.id,
													)
												}
											>
												Xóa
											</button>
										</td>
									</tr>
								))
							) : (
								<tr>
									<td colSpan={4}>
										<h4 className='text-center text-danger'>
											Không tìm thấy chủ đề
										</h4>
									</td>
								</tr>
							)}
						</tbody>
					</Table>
					<Pager
						metadata={metadata}
						onPageChange={handleChangePage}
					/>
				</>
			)}
		</div>
	);
}
