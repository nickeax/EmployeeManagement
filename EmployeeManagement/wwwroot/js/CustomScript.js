function confirmDelete(id, isDeleteClicked) {
    console.log(id)
    if (isDeleteClicked) {
        $(`#deleteSpan_${id}`).hide()
        $(`#confirmSpan_${id}`).show()
    } else {
        $(`#deleteSpan_${id}`).show()
        $(`#confirmSpan_${id}`).hide()
    }
}