// Libraries
import { useEffect, useState } from 'react';
import { Button, Table } from 'react-bootstrap';

// App's features
import {
	deleteCommentById,
	getCommentsByQueries,
	toggleCommentApprovalStatus,
} from '../../../services/comments';

// App's components
import Pager from '../../../components/blog/Pager';
import Loading from '../../../components/Loading';
import CommentFilterPane from '../../../components/admin/CommentFilterPane';
import { getPosts } from '../../../services/posts';

export default function Comments() {
	// Component's states
	const [pageNumber, setPageNumber] = useState(1);
	const [comments, setComments] = useState([]);
	const [isLoading, setIsLoading] = useState(true);
	const [metadata, setMetadata] = useState({});
	const [keyword, setKeyword] = useState('');
	const [postId, setPostId] = useState();
	const [year, setYear] = useState();
	const [month, setMonth] = useState();
	const [isNotApproved, setIsNotApproved] = useState(false);
	const [isChangeStatus, setIsChangeStatus] = useState(false);

	// Component's event handlers
	const handleChangePage = (value) => {
		setPageNumber((current) => current + value);
		window.scroll(0, 0);
	};
	const handleToggleApprovalStatus = async (e, id) => {
		await toggleCommentApprovalStatus(id);
		setIsChangeStatus(!isChangeStatus);
	};
	const handleDeleteComment = async (e, id) => {
		if (window.confirm('Bạn có chắc muốn xóa bình luận?')) {
			const data = await deleteCommentById(id);
			if (data.isSuccess) alert(data.result);
			else alert(data.errors[0]);
			setIsChangeStatus(!isChangeStatus);
		}
	};

	useEffect(() => {
		document.title = 'Danh sách bình luận';
		fetchComments();

		async function fetchComments() {
			const queries = new URLSearchParams({
				IsNotApproved: isNotApproved,
				PageNumber: pageNumber || 1,
				PageSize: 10,
			});
			keyword && queries.append('Keyword', keyword);
			postId && queries.append('PostId', postId);
			year && queries.append('PostedYear', year);
			month && queries.append('PostedMonth', month);

			const data = await getCommentsByQueries(queries);
			const postsData = await getPosts();
			if (data) {
				setComments(
					data.items.map((comment) => ({
						...comment,
						post: postsData.find((p) => p.id === comment.postId),
					})),
				);
				setMetadata(data.metadata);
			} else {
				setComments([]);
				setMetadata({});
			}
			setIsLoading(false);
		}
	}, [
		pageNumber,
		keyword,
		postId,
		year,
		month,
		isNotApproved,
		isChangeStatus,
	]);

	return (
		<div className='mb-5'>
			<h1>Danh sách bình luận</h1>
			<CommentFilterPane
				setKeyword={setKeyword}
				setPostId={setPostId}
				setYear={setYear}
				setMonth={setMonth}
				setIsNotApproved={setIsNotApproved}
			/>
			{isLoading ? (
				<Loading />
			) : (
				<>
					<Table striped responsive bordered>
						<thead>
							<tr>
								<th>Tên người gửi</th>
								<th>Nội dung</th>
								<th>Ngày gửi</th>
								<th>Phê duyệt</th>
								<th>Bài viết</th>
								<th>Xóa</th>
							</tr>
						</thead>
						<tbody>
							{comments.length > 0 ? (
								comments.map((comment) => (
									<tr key={comment.id}>
										<td>
											<p>{comment.name}</p>
										</td>
										<td>{comment.description}</td>
										<td>
											{new Date(
												comment.postedDate,
											).toLocaleDateString('vi-VN')}
										</td>
										<td>
											{comment.isApproved ? (
												<Button
													variant='primary'
													onClick={(e) =>
														handleToggleApprovalStatus(
															e,
															comment.id,
														)
													}
												>
													Có
												</Button>
											) : (
												<Button
													variant='secondary'
													onClick={(e) =>
														handleToggleApprovalStatus(
															e,
															comment.id,
														)
													}
												>
													Không
												</Button>
											)}
										</td>
										<td>{comment.post.title}</td>
										<td>
											<button
												onClick={(e) =>
													handleDeleteComment(
														e,
														comment.id,
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
											Không tìm thấy bình luận
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
