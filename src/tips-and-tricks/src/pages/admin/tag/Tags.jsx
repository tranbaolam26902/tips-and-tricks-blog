// Libraries
import { useEffect, useState } from 'react';
import { Table } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// App's features
import { deleteTagById, getTagsByQueries } from '../../../services/tags';

// App's components
import Loading from '../../../components/Loading';
import Pager from '../../../components/blog/Pager';
import TagFilterPane from '../../../components/admin/TagFilterPane';

export default function Tags() {
	// Component's states
	const [pageNumber, setPageNumber] = useState(1);
	const [keyword, setKeyword] = useState('');
	const [isLoading, setIsLoading] = useState(true);
	const [metadata, setMetadata] = useState({});
	const [tags, setTags] = useState([]);
	const [isChangeStatus, setIsChangeStatus] = useState(false);

	// Component's event handlers
	const handleChangePage = (value) => {
		setPageNumber((current) => current + value);
		window.scroll(0, 0);
	};
	const handleDeleteTag = async (e, id) => {
		if (window.confirm('Bạn có chắc muốn xóa thẻ?')) {
			const data = await deleteTagById(id);
			if (data.isSuccess) alert(data.result);
			else alert(data.errors[0]);
			setIsChangeStatus(!isChangeStatus);
		}
	};

	useEffect(() => {
		document.title = 'Danh sách thẻ';
		fetchTags();

		async function fetchTags() {
			const queries = new URLSearchParams({
				PageNumber: pageNumber || 1,
				PageSize: 10,
			});
			keyword && queries.append('Keyword', keyword);

			const data = await getTagsByQueries(queries);
			if (data) {
				setTags(data.items);
				setMetadata(data.metadata);
			} else {
				setTags([]);
				setMetadata({});
			}
			setIsLoading(false);
		}
	}, [pageNumber, keyword, isChangeStatus]);

	return (
		<div className='mb-5'>
			<h1>Danh sách thẻ</h1>
			<TagFilterPane setKeyword={setKeyword} />
			{isLoading ? (
				<Loading />
			) : (
				<>
					<Table striped responsive bordered>
						<thead>
							<tr>
								<th>Thẻ</th>
								<th>Mô tả</th>
								<th>Số bài viết</th>
								<th>Xóa</th>
							</tr>
						</thead>
						<tbody>
							{tags.length > 0 ? (
								tags.map((tag) => (
									<tr key={tag.id}>
										<td>
											<Link
												to={`/admin/tags/edit/${tag.id}`}
												className='text-bold'
											>
												{tag.name}
											</Link>
										</td>
										<td>{tag.description}</td>
										<td>{tag.postCount}</td>
										<td>
											<button
												onClick={(e) =>
													handleDeleteTag(e, tag.id)
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
											Không tìm thấy thẻ
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
