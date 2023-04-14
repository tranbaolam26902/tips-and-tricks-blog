// Libraries
import { useEffect, useState } from 'react';
import { Table } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// App's features
import {
	deleteAuthorById,
	getAuthorsByQueries,
} from '../../../services/authors';

// App's components
import Pager from '../../../components/blog/Pager';
import Loading from '../../../components/Loading';
import AuthorFilterPane from '../../../components/admin/AuthorFilterPane';

export default function Posts() {
	// Component's states
	const [pageNumber, setPageNumber] = useState(1);
	const [authors, setAuthors] = useState([]);
	const [isLoading, setIsLoading] = useState(true);
	const [metadata, setMetadata] = useState({});
	const [keyword, setKeyword] = useState('');
	const [year, setYear] = useState();
	const [month, setMonth] = useState();
	const [isChangeStatus, setIsChangeStatus] = useState(false);

	// Component's event handlers
	const handleChangePage = (value) => {
		setPageNumber((current) => current + value);
		window.scroll(0, 0);
	};
	const handleDeleteAuthor = async (e, id) => {
		if (window.confirm('Bạn có chắc muốn xóa tác giả?')) {
			const data = await deleteAuthorById(id);
			if (data.isSuccess) alert(data.result);
			else alert(data.errors[0]);
			setIsChangeStatus(!isChangeStatus);
		}
	};

	useEffect(() => {
		document.title = 'Danh sách tác giả';
		fetchAuthors();

		async function fetchAuthors() {
			const queries = new URLSearchParams({
				PageNumber: pageNumber || 1,
				PageSize: 10,
			});
			keyword && queries.append('Keyword', keyword);
			year && queries.append('JoinedYear', year);
			month && queries.append('JoinedMonth', month);

			const data = await getAuthorsByQueries(queries);
			if (data) {
				setAuthors(data.items);
				setMetadata(data.metadata);
			} else {
				setAuthors([]);
				setMetadata({});
			}
			setIsLoading(false);
		}
	}, [pageNumber, keyword, year, month, isChangeStatus]);

	return (
		<div className='mb-5'>
			<h1>Danh sách tác giả</h1>
			<AuthorFilterPane
				setKeyword={setKeyword}
				setYear={setYear}
				setMonth={setMonth}
			/>
			{isLoading ? (
				<Loading />
			) : (
				<>
					<Table striped responsive bordered>
						<thead>
							<tr>
								<th>Ảnh đại diện</th>
								<th>Tên</th>
								<th>Ngày tham gia</th>
								<th>Email</th>
								<th>Notes</th>
								<th>Xóa</th>
							</tr>
						</thead>
						<tbody>
							{authors.length > 0 ? (
								authors.map((author) => (
									<tr key={author.id}>
										<td>
											<img
												src={
													process.env
														.REACT_APP_API_ROOT_URL +
													author.imageUrl
												}
												alt='author'
											/>
										</td>
										<td>
											<Link
												to={`/admin/authors/edit/${author.id}`}
												className='text-bold'
											>
												{author.fullName}
											</Link>
										</td>
										<td>
											{new Date(
												author.joinedDate,
											).toLocaleDateString('vi-VN')}
										</td>
										<td>{author.email}</td>
										<td>{author.notes}</td>
										<td>
											<button
												onClick={(e) =>
													handleDeleteAuthor(
														e,
														author.id,
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
									<td colSpan={6}>
										<h4 className='text-center text-danger'>
											Không tìm thấy tác giả
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
