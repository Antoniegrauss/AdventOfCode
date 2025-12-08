add_executable(day8)
target_sources(day8
    PRIVATE
    day8.cpp)
target_link_libraries(day8 read_file)
target_include_directories(day8 PRIVATE ../utils)
